using System;
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
    [Authorize]
    public class PlayController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public PlayController(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        // GET: api/Play/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Play
        [HttpPost]
        public JsonResult Lapp([FromBody] string command,[FromBody]  int lappId)
        {
            var gameCookie = Request.GetCookie();

            if (!gameCookie?.IsInGame == true)
            {
                throw new SystemException("Fel i gameCookie");
            }

            // Hämta random återstående lapp
            var game = _dbContext.Games.Include(g => g.Lapps).Single(g => g.GameID == gameCookie.GameId);

            switch (command)
            {
                case "claim":
                    game.ClaimLapp(gameCookie.PlayerId, lappId);
                    break;
                case "skip":
                    game.SkipLapp(gameCookie.PlayerId, lappId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("command");
            }

            var lapps = game.Lapps;

            var count = lapps.Count;

            if (count == 0)
            {
                return null;
            }

            var selected = game.GetNextLapp();

            return new JsonResult(new {lappId = selected.LappID, lappContent = selected.Text});

        }

        // PUT: api/Play/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
