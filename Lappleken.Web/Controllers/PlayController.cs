﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lappleken.Web.Data;
using Lappleken.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public PlayController(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        [HttpGet]
        [Route("GameStatus")]
        public JsonResult GameStatus()
        {
            var gameCookie = Request.GetCookie();

            if (!gameCookie?.IsInGame == true)
            {
                throw new SystemException("Fel i gameCookie");
            }

            var game = _dbContext.Games.Single(g => g.GameID == gameCookie.GameId);

            var status = game.GetStatus();

            return new JsonResult(new {status.Phase, status.ActivePlayerId, status.RemainingTimeForPlayer});
        }

        [HttpPost( Name = "Lapp")]
        [Route("Lapp")]
        public JsonResult Lapp([FromForm] LappCommand lappCommand)
        {
            var gameCookie = Request.GetCookie();

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

            var selected = game.GetNextLapp();

            _dbContext.SaveChanges();

            if (selected == null)
            {
                return new JsonResult(new{});
            }

            return new JsonResult(new {lappId = selected.LappID, lappContent = selected.Text});

        } 
        
        // POST: api/Play
        [HttpPost(Name = "TakeBowl")]
        [Route("TakeBowl")]
        public ActionResult TakeBowl()
        {
            var gameCookie = Request.GetCookie();

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
