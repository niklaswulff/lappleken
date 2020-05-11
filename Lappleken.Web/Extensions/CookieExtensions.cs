using Lappleken.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Lappleken.Web.Extensions
{
    public static class CookieExtensions
    {
        public static void SetGameCookie(this HttpResponse response, string userId, int? gameId, int? playerID,
            string username,
            int? teamId)
        {
            var gameCookie = new GameController.GameCookie(){GameId = gameId,PlayerId = playerID, PlayerName = username, TeamId = teamId};
            response.Cookies.Append("gameCookie" + userId, JsonConvert.SerializeObject(gameCookie));

        }

        public static void RemoveGameCookie(this HttpResponse response, string userId)
        {
            response.Cookies.Delete("gameCookie" + userId);
        }

        public static GameController.GameCookie GetGameCookie(this HttpRequest request, string userId)
        {
            var cookie = request.Cookies["gameCookie" + userId];

            if (cookie == null)
            {
                return null;
            }

            return (GameController.GameCookie) JsonConvert.DeserializeObject(cookie, typeof(GameController.GameCookie));
        }
    }
}