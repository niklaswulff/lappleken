using System.Collections.Generic;
using Lappleken.Controllers;

namespace Lappleken.Models
{
    public class GameViewModel
    {
        public string Name { get; set; }
        public string TeamToAdd { get; set; }
        public List<TeamViewModel> Teams { get; set; }
        public int GameId { get; set; }
    }
}