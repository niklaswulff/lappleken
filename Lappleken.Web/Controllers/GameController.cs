using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lappleken.Web.Data;
using Lappleken.Web.Extensions;
using Lappleken.Web.Models;
using Lappleken.Web.Models.Game;
using Lappleken.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Lappleken.Web.Controllers
{
    [Authorize]
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
            var game = _dbContext.Games.Include(g => g.Teams).Include(g => g.Lapps).Single(g => g.GameID == id);

            var teamViewModels = game.Teams?.Select(t => new TeamViewModel(t.TeamID, t.Name)).ToList() ?? new List<TeamViewModel>();

            return View(new GameDetailsViewModel() { GameId = id, Name = "", Teams = teamViewModels });

        }

        [HttpGet]
        public ActionResult Join(GameTeam gameTeam)
        {
            var gameCookie = Request.GetCookie();

            if (gameCookie?.IsInGame == true)
            {
                return RedirectToAction("Play", "Game", new {id = gameCookie.GameId.Value});
            }

            var team = _dbContext.Teams.Include(t => t.Players).Single(t => t.TeamID == gameTeam.TeamId);
            var username = User.GetUsername();
            var player = team.AddPlayer(username);

            _dbContext.SaveChanges();

            Response.AddCookie(gameTeam.GameId, player.PlayerID, username, gameTeam.TeamId);


            return RedirectToAction("Play", "Game", new { id = gameTeam.GameId });
        }

        public ActionResult Play(int id)
        {
            var gameCookie = Request.GetCookie();

            if (!gameCookie?.IsInGame == true)
            {
                throw new SystemException("Fel i gameCookie");
            }

            var player = _dbContext.Players.Include(p => p.CreatedLapps).Single(p => p.PlayerID == gameCookie.PlayerId);

            var team = _dbContext.Teams.Single(t => t.TeamID == gameCookie.TeamId.Value);

            if (player.IsReady)
            {
                return View(new GamePlayViewModel(id, team.Name, gameCookie.PlayerId.Value, gameCookie.PlayerName));
            }

            return RedirectToAction("CreateLapps", new {id});
        }

        public class GameTeam
        {
            [BindProperty(Name = "id")]
            public int GameId { get; set; }

            public int TeamId { get; set; }
        }

        public class GameCookie
        {
            public int? GameId;
            public int? TeamId;
            public int? PlayerId;
            public string PlayerName;

            public bool IsInGame =>
                GameId.HasValue &&
                TeamId.HasValue &&
                PlayerId.HasValue &&
                !string.IsNullOrEmpty(PlayerName);
        }

        [HttpGet]
        public ActionResult CreateLapps(int id)
        {
            var gameCookie = Request.GetCookie();

            if (!gameCookie?.IsInGame == true)
            {
                throw new SystemException("Fel i gameCookie");
            }

            return View(new GameCreateLappsViewModel(){GameId = id, LappTexts = new string[8], PlayerId = gameCookie.PlayerId.Value});
        }

        [HttpPost]
        public ActionResult CreateLapps(GameCreateLappsViewModel postedModel)
        {
            var game = _dbContext.Games.Single(g => g.GameID == postedModel.GameId);
            var player = _dbContext.Players.Single(p => p.PlayerID == postedModel.PlayerId);

            foreach (var lappText in postedModel.LappTexts)
            {
                game.AddLapp(player, lappText);
            }

            return RedirectToAction("Play", "Game");
        }
    }
}