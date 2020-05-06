using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Lappleken.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lappleken.Web.Models;
using Lappleken.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;

namespace Lappleken.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            var gameId = Request.Cookies["game-id"];
            var teamName = Request.Cookies["my-team"];
            var playerName = Request.Cookies["player-name"];

            if (!string.IsNullOrWhiteSpace(gameId) && !string.IsNullOrWhiteSpace(teamName) &&
                !string.IsNullOrWhiteSpace(playerName))
            {
               // return RedirectToAction("Play", new {id = gameId});
            }

            var activeGames = _dbContext.Games.Where(g => g.Created).ToList();
            return View(new IndexViewModel() { ActiveGames = activeGames });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
