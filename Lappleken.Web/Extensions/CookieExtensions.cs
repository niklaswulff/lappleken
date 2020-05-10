using Lappleken.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Lappleken.Web.Extensions
{
    public static class CookieExtensions
    {
        public static void AddCookie(this HttpResponse response, int? gameId, int? playerID, string username,
            int? teamId)
        {
            var gameCookie = new GameController.GameCookie(){GameId = gameId,PlayerId = playerID, PlayerName = username, TeamId = teamId};
            response.Cookies.Append("gameCookie", JsonConvert.SerializeObject(gameCookie));

        }

        public static GameController.GameCookie GetCookie(this HttpRequest request)
        {
            var cookie = request.Cookies["gameCookie"];

            if (cookie == null)
            {
                return null;
            }

            return (GameController.GameCookie) JsonConvert.DeserializeObject(cookie, typeof(GameController.GameCookie));
        }
    }
}