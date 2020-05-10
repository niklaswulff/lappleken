using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Lappleken.Web.Data.Model
{
    public class Team
    {
        private List<Player> _players;
        private Team() { }
        public Team(string name)
        {
            this._players = new List<Player>();
            this.Name = name;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamID { get; private set; }
        public string Name { get; set; }
        public IReadOnlyCollection<Player> Players => _players;

        public Player AddPlayer(string name)
        {
            if (this._players.Select(p => p.Name).Contains(name))
            {
                throw new ArgumentException(nameof(name));
            }

            var player = new Player(name);
            this._players.Add(player);

            return player;
        }
    }
}