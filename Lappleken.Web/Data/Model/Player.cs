using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lappleken.Web.Data.Model
{
    public class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerID { get; private set; }
        public string Name { get; private set; }

        private List<Lapp> _createdLapps;
        public IReadOnlyCollection<Lapp> CreatedLapps => _createdLapps.AsReadOnly();

        public bool IsReady
        {
            get
            {
                return _createdLapps.Count == 8;
            }
        }

        protected Player(){}

        public Player(string name)
        {
            Name = name;
            _createdLapps = new List<Lapp>();
        }
    }
}