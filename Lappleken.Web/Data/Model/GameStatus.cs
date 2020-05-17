namespace Lappleken.Web.Data.Model
{
    public class GameStatus
    {
        public int GameId { get; set; }
        public string Phase { get; set; }
        public int? ActivePlayerId { get; set; }

        public int? RemainingTimeForPlayer { get; set; }
        public int? ActiveTeamId { get; set; }
        public bool ActivePlayerDone { get; set; }
        public string GameState { get; set; }
    }
}