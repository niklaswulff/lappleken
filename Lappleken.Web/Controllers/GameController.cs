using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lappleken.Web.Data;
using Lappleken.Web.Extensions;
using Lappleken.Web.Models.Game;
using Lappleken.Web.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lappleken.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public GameController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(int id)
        {
            var game = _dbContext.Games.Include("Teams").Include("Lapps").Single(g => g.GameID == id);

            return View(new GameDetailsViewModel() { GameId = id, Name = "", Teams = game.Teams?.Select(t => new TeamViewModel(t.TeamID, t.Name)).ToList() ?? new List<TeamViewModel>() });

        }

        [HttpGet]
        public ActionResult Join(GameTeam gameTeam)
        {
            var team = _dbContext.Teams.Include(t => t.Players).Single(t => t.TeamID == gameTeam.TeamId);
            var username = User.GetUsername();
            var player = team.AddPlayer(username);

            _dbContext.SaveChanges();

            Response.Cookies.Append("game-id", gameTeam.GameId.ToString());
            Response.Cookies.Append("my-team", gameTeam.TeamId.ToString());
            Response.Cookies.Append("player-id", player.PlayerID.ToString());
            Response.Cookies.Append("player-name",username);

            return RedirectToAction("Play", "Game", new {id = gameTeam.GameId});
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

            return View(new GamePlayViewModel(id, teamName, playerId, playerName));
        }



        public class GameTeam
        {
            [BindProperty(Name = "id")]
            public int GameId { get; set; }

            public int TeamId { get; set; }
        }

    }
}