using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lappleken.Web.Data;
using Lappleken.Web.Data.Model;
using Lappleken.Web.Extensions;
using Lappleken.Web.Models;
using Lappleken.Web.Models.Game;
using Lappleken.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Lappleken.Web.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private string UserId => _userManager.GetUserId(User);
        private const int NumberOfLapps = 8;

        public GameController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
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
            var gameCookie = Request.GetGameCookie(UserId);

            if (gameCookie?.IsInGame == true)
            {
                return RedirectToAction("Play", "Game");
            }

            var team = _dbContext.Teams.Include(t => t.Players).Single(t => t.TeamID == gameTeam.TeamId);
            var username = User.GetUsername();
            var player = team.AddPlayer(UserId, username);

            _dbContext.SaveChanges();

            Response.SetGameCookie(UserId, gameTeam.GameId, player.PlayerID, username, gameTeam.TeamId);
            
            return RedirectToAction("Play", "Game");
        }

        [HttpGet]
        public ActionResult LeaveGame()
        {
            var gameCookie = Request.GetGameCookie(UserId);

            var game = _dbContext.Games.Include(g => g.Teams).ThenInclude(t => t.Players)
                .Single(g => g.GameID == gameCookie.GameId);

                game.RemovePlayerFromTeam(gameCookie.PlayerId.Value, gameCookie.TeamId.Value);

                _dbContext.SaveChanges();

                Response.RemoveGameCookie(UserId);

                return RedirectToAction("Index", "Home");
        }


        public ActionResult Play(int id)
        {
            var gameCookie = Request.GetGameCookie(UserId);

            if (gameCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }
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

            return RedirectToAction("CreateLapps", new { id });
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
            var gameCookie = Request.GetGameCookie(UserId);

            if (!gameCookie?.IsInGame == true)
            {
                throw new SystemException("Fel i gameCookie");
            }

            return View(new GameCreateLappsViewModel() { GameId = gameCookie.GameId.Value, LappTexts = new string[NumberOfLapps], PlayerId = gameCookie.PlayerId.Value });
        }

        [HttpPost]
        public ActionResult CreateLapps(GameCreateLappsViewModel postedModel)
        {
            var game = _dbContext.Games.Include(g => g.Lapps).Single(g => g.GameID == postedModel.GameId);
            var player = _dbContext.Players.Single(p => p.PlayerID == postedModel.PlayerId);

            for (var index = 0; index < postedModel.LappTexts.Length; index++)
            {
                var lappText = postedModel.LappTexts[index];
                if (string.IsNullOrWhiteSpace(lappText))
                {
                    ModelState.AddModelError(nameof(postedModel.LappTexts) + "[" + index + "]", "Får inte vara tom");
                }
                else
                {
                    game.AddLapp(player, lappText);
                }
            }

            if (!ModelState.IsValid)
            {
                return View(postedModel);
            }

            _dbContext.SaveChanges();

            return RedirectToAction("Play", "Game");
        }

        public IActionResult Create()
        {
            var newGame = new Game(UserId);

            _dbContext.Games.Add(newGame);

            _dbContext.SaveChanges();

            return RedirectToAction("Details", "Game", new { id = newGame.GameID });
        }

        public IActionResult CreateTeam(GameDetailsViewModel postedModel)
        {
            var game = _dbContext.Games.Include(g => g.Teams).Single(g => g.GameID == postedModel.GameId);

            var team = game.AddTeam(postedModel.TeamToAdd);

            var player = team.AddPlayer(UserId, User.GetUsername());

            _dbContext.SaveChanges();

            Response.SetGameCookie(UserId, postedModel.GameId, player.PlayerID, player.Name, team.TeamID);

            return RedirectToAction("Play", "Game");
        }
    }
}