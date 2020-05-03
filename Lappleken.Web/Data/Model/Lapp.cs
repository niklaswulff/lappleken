using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Lappleken.Web.Data.Model
{
    public class Lapp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LappID { get; private set; }
        public string Text { get; private set; }

        public Lapp(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.Text = text;
        }

        private Lapp()
        {

        }
    }

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

        public void AddLapp(string text)
        {
            if (this.Started)
            {
                throw new Exception("Started");
            }
            this._lapps.Add(new Lapp(text));
        }
    }

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

    public class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerID { get; private set; }
        public string Name { get; private set; }

        protected Player(){}

        public Player(string name)
        {
            Name = name;
        }
    }
}
