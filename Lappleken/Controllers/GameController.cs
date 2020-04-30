using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Model;
using Lappleken.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lappleken.Controllers
{
    public class GameController : Controller
    {
        private readonly LappContext _lappContext;

        public GameController(LappContext lappContext)
        {
            _lappContext = lappContext;
        }

        // GET: Game
        public ActionResult Index()
        {
            var gameId = Request.Cookies["game-id"];
            var teamName = Request.Cookies["my-team"];
            var playerName = Request.Cookies["player-name"];

            if (!string.IsNullOrWhiteSpace(gameId) && !string.IsNullOrWhiteSpace(teamName) &&
                !string.IsNullOrWhiteSpace(playerName))
            {
                return RedirectToAction("Play", new {id = gameId});
            }

            var activeGames = _lappContext.Games.Where(g => g.Created).ToList();
            return View(new IndexViewModel() { ActiveGames = activeGames });
        }

        // GET: Game/Details/5
        public ActionResult Details(int id)
        {
            var game = _lappContext.Games.Include("Teams").Include("Lapps").Single(g => g.GameID == id);

            return View(new GameViewModel() { GameId = id, Name = "", Teams = game.Teams?.Select(t => new TeamViewModel(t.TeamID, t.Name)).ToList() ?? new List<TeamViewModel>() });
        }

        [HttpPost]
        public ActionResult Details(GameViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.TeamToAdd))
            {
                throw new ArgumentNullException(nameof(model.TeamToAdd));
            }

            var game = _lappContext.Games.Include("Teams").Include("Lapps").Single(g => g.GameID == model.GameId);

            game.AddTeam(model.TeamToAdd);

            _lappContext.SaveChanges();

            return RedirectToAction(nameof(Details), new {id = model.GameId});
        }

        // POST: Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var game = new Game("");
                _lappContext.Games.Add(game);

                _lappContext.SaveChanges();

                return RedirectToAction(nameof(Details), new { id = game.GameID });
            }
            catch
            {
               return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Join(GameTeam gameTeam)
        {
            return View(new JoinViewModel() {GameId = gameTeam.GameId, TeamId = gameTeam.TeamId});
        }
        
        [HttpPost]
        public ActionResult Join(JoinViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.PlayerName))
            {
                throw new ArgumentNullException(nameof(model.PlayerName));
            }

            var team = _lappContext.Teams.Include(t => t.Players).Single(t => t.TeamID == model.TeamId);
            var player = team.AddPlayer(model.PlayerName);

            _lappContext.SaveChanges();

            Response.Cookies.Append("game-id", model.GameId.ToString());
            Response.Cookies.Append("my-team", model.TeamId.ToString());
            Response.Cookies.Append("player-id", player.PlayerID.ToString());
            Response.Cookies.Append("player-name", model.PlayerName);

            return RedirectToAction("Play", "Game", new {id = model.GameId});
        }

        public ActionResult Play(int id)
        {
            var teamName = Request.Cookies["my-team"];
            var playerId = Request.Cookies["player-id"];
            var playerName = Request.Cookies["player-name"];

            if (string.IsNullOrWhiteSpace(teamName))
            {
                throw new ArgumentNullException("my-team cookie");
            }

            return View(new PlayViewModel(id, teamName, playerId, playerName));
        }

        public ActionResult Lapp(int id)
        {
            // Hämta random återstående lapp
            var lapps = _lappContext.Games.Single(g => g.GameID == id).Lapps;
            var count = lapps.Count;
            var selected = lapps.Skip(new Random().Next(count)).Take(1).First();

            return new JsonResult(new {lappId = selected.LappID, lappContent = selected.Text});
        }

        [HttpPost]
        public ActionResult Lapp(int id, string command)
        {
            // Hantera command

            return RedirectToAction("Lapp", new {id});
        }
    }

    public class TeamViewModel
    {
        public int TeamId { get; }
        public string Name { get; }

        public TeamViewModel(int teamId, string name)
        {
            TeamId = teamId;
            Name = name;
        }
    }

    public class JoinViewModel
    {
        public int GameId { get; set; }
        public int TeamId { get; set; }
        public string PlayerName { get; set; }
    }

    public class PlayViewModel
    {
        public PlayViewModel(in int id, string teamName, string playerId, string playerName)
        {
            this.GameId = id;
            this.TeamName = teamName;
            PlayerId = playerId;
            PlayerName = playerName;
        }

        public int GameId { get; set; }
        public string TeamName { get; set; }
        public string PlayerId { get; }
        public string PlayerName { get; }

    }
}