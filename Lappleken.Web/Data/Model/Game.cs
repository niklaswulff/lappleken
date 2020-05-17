using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Lappleken.Web.Data.Model
{
    public class Game
    {
        public enum PhaseEnum
        {
            NotStarted = 0,
            ManyWords = 1,
            OnlyOneWord = 2,
            Charades = 3,
            Humming = 4,
            Ended = 5
        }

        public enum GameState
        {
            WaitingForGameToStart,
            WaitingForPlayerToStart,
            PlayerIsActive,
            WaitingForPlayerToTakeBowl,
            WaitingForPhaseToStart,
            GameIsEnded
        }

        public const string CommandClaim = "claim";
        public const string CommandSkip = "skip";
        public const string CommandFirst = "first";

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameID { get; private set; }

        public DateTime Date { get; private set; }

        public IReadOnlyCollection<Team> Teams => _teams?.ToList();
        public IReadOnlyCollection<Lapp> Lapps => _lapps?.ToList();

        public string CreatedBy { get; private set; }

        public PhaseEnum Phase { get; private set; }
        public bool PhaseStarted { get; private set; }
        public DateTime? PlayerEndsAt { get; private set; }

        public int? ActiveTeamId { get; private set; }
        public int? ActivePlayerId { get; private set; }
        public int? ActivePlayerRemainingTime { get; private set; }
        public bool ActivePlayerDone { get; private set; }

        public bool Started => this.Phase != PhaseEnum.NotStarted;

        private List<Team> _teams;
        private List<Lapp> _lapps;

        protected Game() { }

        public Game(string createdByUserId)
        {
            _teams = new List<Team>();
            _lapps = new List<Lapp>();

            Date = DateTime.Now;
            CreatedBy = createdByUserId;
        }

        public GameStatus GetStatus()
        {
            return new GameStatus
            {
                GameState = State.ToString(),
                GameId = GameID,
                Phase = Phase.ToString(),
                ActiveTeamId = ActiveTeamId,
                ActivePlayerId = ActivePlayerId,
                ActivePlayerDone = ActivePlayerDone,
                RemainingTimeForPlayer = RemainingSecondsForActivePlayer,
            };
        }

        public GameState State
        {
            get
            {
                if (ActivePlayerId.HasValue)
                {
                    return PlayerEndsAt.HasValue ? GameState.PlayerIsActive : GameState.WaitingForPlayerToStart;
                }

                if (Phase == PhaseEnum.NotStarted)
                {
                    return GameState.WaitingForGameToStart;
                }

                if (Phase == PhaseEnum.Ended)
                {
                    return GameState.GameIsEnded;
                }

                if (PhaseStarted)
                {
                    return GameState.WaitingForPlayerToTakeBowl;
                }

                return GameState.WaitingForPhaseToStart;
            }
        }

        public Team AddTeam(string name)
        {
            var team = new Team(name);
            _teams.Add(team);

            return team;
        }

        public void AddLapp(Player player, string text)
        {
            if (this.Started)
            {
                throw new Exception("Game already started");
            }

            this._lapps.Add(new Lapp(this, player, text));
        }

        public void ClaimLapp(int playerId, int lappId)
        {
            CheckIfTimeIsUp();

            if (ActivePlayerId != playerId)
            {
                throw new SystemException("Fel spelare tar lapp");
            }

            if (lappId == 0)
            {
                throw new SystemException("Ingen lapp att ta");
            }

            var player = _teams.SelectMany(t => t.Players).Single(p => p.PlayerID == playerId);

            this.Lapps.Single(l => l.LappID == lappId).AddLogg(this.Phase, player, CommandClaim);

            if (ActivePlayerDone)
            {
                ActivePlayerId = null;
            }
        }

        public void SkipLapp(int playerId, int lappId)
        {
            CheckIfTimeIsUp();

            if (ActivePlayerId != playerId)
            {
                throw new SystemException("Fel spelare skippar lapp");
            }

            if (lappId == 0)
            {
                throw new SystemException("Ingen lapp att lägga tillbaka");
            }


            var player = _teams.SelectMany(t => t.Players).Single(p => p.PlayerID == playerId);

            this.Lapps.Single(l => l.LappID == lappId).AddLogg(this.Phase, player, CommandSkip);

            if (ActivePlayerDone)
            {
                ActivePlayerId = null;
            }
        }

        public Lapp GetNextLapp(int playerId)
        {
            if (ActivePlayerId != playerId)
            {
                return null;
            }

            var unclaimed = _lapps.Where(l => l.ClaimedInPhase == null || l.ClaimedInPhase < Phase).ToList();

            if (!unclaimed.Any())
            {
                MoveToNextPhase();

                return null;
            }

            var count = unclaimed.Count;

            return unclaimed.Skip(new Random().Next(count)).Take(1).First();
        }

        public void BowlToPlayer(int playerId)
        {
            // Kolla att det är rätt team
            var playerTeamId = GetPlayerTeamId(playerId);
            if (!ActiveTeamId.HasValue)
            {
                ActiveTeamId = playerTeamId;
            }
            else if (playerTeamId != ActiveTeamId)
            {
                throw new SystemException("Spelare i fel lag försökte ta skålen");
            }

            ActivePlayerId = playerId;
            ActivePlayerDone = false;
            PlayerEndsAt = null;
        }

        public void PlayerStarted(int playerId)
        {
            if (ActivePlayerId != playerId)
            {
                throw new SystemException("Fel player är active");
            }

            if (Phase == PhaseEnum.NotStarted)
            {
                StartGame();
            }

            PhaseStarted = true;

            SetPlayerEndTime();
        }

        public void RemovePlayerFromTeam(int playerId, int teamId)
        {
            var team = Teams.Single(t => t.TeamID == teamId);

            team.RemovePlayer(playerId);
        }

        private void SetPlayerEndTime()
        {
            //PlayerEndsAt = DateTime.Now.AddMinutes(1);
            PlayerEndsAt = DateTime.Now.AddSeconds(10);
        }

        private void CheckIfTimeIsUp()
        {
            if (RemainingSecondsForActivePlayer == 0 && !ActivePlayerDone)
            {
                ActivePlayerDone = true;
                SetNextTeamActive();
            }
        }

        private void MoveToNextPhase()
        {
            Phase++;
            PhaseStarted = false;
            ActivePlayerRemainingTime = RemainingSecondsForActivePlayer;
        }

        private void StartGame()
        {
            Phase = PhaseEnum.ManyWords;
        }

        private void SetNextTeamActive()
        {
            var teamIds = _teams.Select(t => t.TeamID).OrderBy(t => t).ToArray();

            var i = Array.IndexOf(teamIds, ActiveTeamId);

            if (i == teamIds.Length - 1)
            {
                ActiveTeamId = teamIds[0];
            }
            else
            {
                ActiveTeamId = teamIds[i + 1];
            }
        }

        private int? RemainingSecondsForActivePlayer
        {
            get
            {
                var remainingTimeForPlayer = PlayerEndsAt - DateTime.Now;

                if (remainingTimeForPlayer.HasValue && remainingTimeForPlayer.Value < TimeSpan.Zero)
                {
                    remainingTimeForPlayer = TimeSpan.Zero;
                }

                return remainingTimeForPlayer?.Seconds;
            }
        }

        private int GetPlayerTeamId(int playerId)
        {
            var players = _teams.SelectMany(t => t.Players).ToList();
            return players.Single(p => p.PlayerID == playerId).TeamID.Value;
        }
    }
}