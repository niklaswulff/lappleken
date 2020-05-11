using System.Diagnostics;
using System.Linq;
using Lappleken.Web.Data;
using Lappleken.Web.Data.Model;
using Lappleken.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lappleken.Web.Models;
using Lappleken.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Lappleken.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private string UserId => _userManager.GetUserId(User);

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            var gameCookie = Request.GetGameCookie(UserId);

            if (gameCookie?.IsInGame == true)
            {
                return RedirectToAction("Play", "Game", new {id = gameCookie.GameId.Value});
            }

            var activeGames = _dbContext.Games.Where(g => g.Phase == Game.PhaseEnum.NotStarted).ToList();
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
