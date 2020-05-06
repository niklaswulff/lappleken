using System.Collections.Generic;
using Lappleken.Web.Models.Home;

namespace Lappleken.Web.Models.Game
{
    public class GameDetailsViewModel
    {
        public string Name { get; set; }
        public string TeamToAdd { get; set; }
        public List<TeamViewModel> Teams { get; set; }
        public int GameId { get; set; }
    }
}