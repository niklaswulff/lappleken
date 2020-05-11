using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Lappleken.Web.Data.Model
{
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameID { get; private set; }

        private List<Team> _teams;
        private List<Lapp> _lapps;

        public DateTime Date { get; private set; }

        public bool Started => this.Phase != PhaseEnum.NotStarted;

        public IReadOnlyCollection<Team> Teams => _teams?.ToList();
        public IReadOnlyCollection<Lapp> Lapps => _lapps?.ToList();
        public bool Created { get; private set; }
        public PhaseEnum Phase { get; private set; }
        public string CreatedBy { get; private set; }

        public DateTime? PlayerStartedAt { get; private set; }

        public int? ActivePlayerId { get; private set; }

        protected Game() { }

        public Game(string createdByUserId)
        {
            _teams = new List<Team>();
            _lapps = new List<Lapp>();

            Date = DateTime.Now;
            Created = true;
            CreatedBy = createdByUserId;
        }

        public GameStatus GetStatus()
        {
            var remainingTimeForPlayer = PlayerStartedAt?.AddMinutes(1) - DateTime.Now;

            if (remainingTimeForPlayer.HasValue && remainingTimeForPlayer.Value < TimeSpan.Zero)
            {
                remainingTimeForPlayer = TimeSpan.Zero;
            }

            return new GameStatus
            {
                GameId = this.GameID,
                Phase = Phase.ToString(),
                ActivePlayerId = ActivePlayerId,
                RemainingTimeForPlayer = remainingTimeForPlayer?.Seconds,
            };
        }

        public Team AddTeam(string name)
        {
            var team = new Team(name);
            this._teams.Add(team);

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
            var player = _teams.SelectMany(t => t.Players).Single(p => p.PlayerID == playerId);

            this.Lapps.Single(l => l.LappID == lappId).AddLogg(this.Phase, player, CommandClaim);
        }

        public void SkipLapp(int playerId, int lappId)
        {
            var player = _teams.SelectMany(t => t.Players).Single(p => p.PlayerID == playerId);

            this.Lapps.Single(l => l.LappID == lappId).AddLogg(this.Phase, player, CommandSkip);
        }

        public Lapp GetNextLapp()
        {
            var unclaimed = _lapps.Where(l => l.ClaimedInPhase == null || l.ClaimedInPhase < Phase).ToList();

            if (!unclaimed.Any())
            {
                MoveToNextPhase();

                return null;
            }

            var count = unclaimed.Count;

            return unclaimed.Skip(new Random().Next(count)).Take(1).First();
        }

        private void MoveToNextPhase()
        {
            Phase++;
            ActivePlayerId = null;
            PlayerStartedAt = null;
        }

        public void BowlToPlayer(int playerId)
        {
            ActivePlayerId = playerId;
        }

        public void PlayerStarted(int playerId)
        {
            if (ActivePlayerId != playerId)
            {
                throw new SystemException("Fel player är active");
            }

            if (Phase == PhaseEnum.NotStarted)
            {
                Phase++;
            }

            PlayerStartedAt = DateTime.Now;
        }

        public enum PhaseEnum
        {
            NotStarted = 0,
            ManyWords = 1,
            OnlyOneWord = 2,
            Charades = 3,
            Humming = 4,
            Ended = 5
        }

        public const string CommandClaim = "claim";
        public const string CommandSkip = "skip";
        public const string CommandFirst = "first";

        public void RemovePlayerFromTeam(int playerId, int teamId)
        {
            var team = Teams.Single(t => t.TeamID == teamId);

            team.RemovePlayer(playerId);
        }
    }

    public class GameStatus
    {
        public int GameId { get; set; }
        public string Phase { get; set; }
        public int? ActivePlayerId { get; set; }

        public int? RemainingTimeForPlayer { get; set; }
    }
}