using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Lappleken.Web.Data.Model
{
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameID { get; private set; }
        
        private List<Team> _teams;
        private List<Lapp> _lapps;

        public DateTime Date { get; }
        public bool Started { get; private set; }

        public IReadOnlyCollection<Team> Teams => _teams?.ToList();
        public IReadOnlyCollection<Lapp> Lapps => _lapps?.ToList();
        public bool Created { get; private set; }

        protected Game(){}

        public Game(string name)
        {
            _teams = new List<Team>();
            _lapps = new List<Lapp>();

            this.Date = DateTime.Now;
            this.Created = true;
        }

        public void AddTeam(string name)
        {
            this._teams.Add(new Team(name));
        }

        public void AddLapp(Player player, string text)
        {
            if (this.Started)
            {
                throw new Exception("Started");
            }
            this._lapps.Add(new Lapp(this, player, text));
        }
    }
}