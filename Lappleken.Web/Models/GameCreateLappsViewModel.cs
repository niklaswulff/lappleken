using System.Collections.Generic;

namespace Lappleken.Web.Models
{
    public class GameCreateLappsViewModel
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public string[] LappTexts { get; set; }

    }
}