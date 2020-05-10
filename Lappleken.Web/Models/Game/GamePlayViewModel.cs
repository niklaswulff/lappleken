namespace Lappleken.Web.Models.Game
{
    public class GamePlayViewModel
    {
        public GamePlayViewModel(in int id, string teamName, int playerId, string playerName)
        {
            this.GameId = id;
            this.TeamName = teamName;
            PlayerId = playerId;
            PlayerName = playerName;
        }

        public int GameId { get; set; }
        public string TeamName { get; set; }
        public int PlayerId { get; }
        public string PlayerName { get; }
        public string Phase { get; }
    }
}