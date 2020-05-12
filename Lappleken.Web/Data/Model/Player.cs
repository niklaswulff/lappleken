using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lappleken.Web.Data.Model
{
    public class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerID { get; private set; }
        public string Name { get; private set; }
        public string UserId { get; private set; }

        private List<Lapp> _createdLapps;
        public int? TeamID { get; private set; }
        public IReadOnlyCollection<Lapp> CreatedLapps => _createdLapps.AsReadOnly();

        public bool IsReady => _createdLapps.Count == 8;
        public bool IsActive { get; set; } = true;

        protected Player() { }

        public Player(string userId, string name)
        {
            UserId = userId;
            Name = name;
            _createdLapps = new List<Lapp>();
        }
    }
}