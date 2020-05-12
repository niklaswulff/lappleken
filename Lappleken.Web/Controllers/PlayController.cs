using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lappleken.Web.Data;
using Lappleken.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lappleken.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayController : ControllerBase
    {
        public class LappCommand
        {
            public string Command { get; set; }
            public int LappId { get; set; }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private string UserId => _userManager.GetUserId(User);

        public PlayController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this._dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GameStatus")]
        public JsonResult GameStatus()
        {
            var gameCookie = Request.GetGameCookie(UserId);

            if (!gameCookie?.IsInGame == true)
            {
                throw new SystemException("Fel i gameCookie");
            }

            var game = _dbContext.Games.Single(g => g.GameID == gameCookie.GameId);

            var status = game.GetStatus();

            var activePlayerName = status.ActivePlayerId.HasValue ? _dbContext.Players.Single(p => p.PlayerID == status.ActivePlayerId).Name : null;
            var activeTeamName = status.ActiveTeamId.HasValue
                ? _dbContext.Teams.Single(t => t.TeamID == status.ActiveTeamId.Value).Name
                : null;

            return new JsonResult(new {status.Phase, activeTeamName, activePlayerName, status.ActivePlayerDone, status.RemainingTimeForPlayer});
        }

        [HttpPost]
        [Route("Lapp")]
        public JsonResult Lapp([FromForm] LappCommand lappCommand)
        {
            var gameCookie = Request.GetGameCookie(UserId);

            if (!gameCookie?.IsInGame == true)
            {
                throw new SystemException("Fel i gameCookie");
            }

            var game = _dbContext.Games.Include(g => g.Teams).ThenInclude(t => t.Players).Include(g => g.Lapps)
                .Single(g => g.GameID == gameCookie.GameId);

            switch (lappCommand.Command)
            {
                case "claim":
                    game.ClaimLapp(gameCookie.PlayerId.Value, lappCommand.LappId);
                    break;
                case "skip":
                    game.SkipLapp(gameCookie.PlayerId.Value, lappCommand.LappId);
                    break;
                case "first":
                    game.PlayerStarted(gameCookie.PlayerId.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("command");
            }

            var selected = game.GetNextLapp(gameCookie.PlayerId.Value);

            _dbContext.SaveChanges();

            if (selected == null)
            {
                return new JsonResult(new{});
            }

            return new JsonResult(new {lappId = selected.LappID, lappContent = selected.Text});

        } 
        
        [HttpPost]
        [Route("TakeBowl")]
        public ActionResult TakeBowl()
        {
            var gameCookie = Request.GetGameCookie(UserId);

            if (!gameCookie?.IsInGame == true)
            {
                throw new SystemException("Fel i gameCookie");
            }

            var game = _dbContext.Games.Include(g => g.Teams).ThenInclude(t => t.Players).Include(g => g.Lapps)
                .Single(g => g.GameID == gameCookie.GameId);

            try
            {
                game.BowlToPlayer(gameCookie.PlayerId.Value);

                _dbContext.SaveChanges();
                return new OkResult();
            }

            catch (Exception exception)
            {
                return new NotFoundResult();
            }
        }
        
    }
}
