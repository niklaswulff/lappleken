using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Lappleken.Web.Data.Model
{
    public class Team
    {
        private List<Player> _players;
        private Team() { }
        public Team(string name)
        {
            _players = new List<Player>();
            Name = name;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamID { get; private set; }
        public string Name { get; set; }
        public IReadOnlyCollection<Player> Players => _players;

        public Player AddPlayer(string userId, string name)
        {
            _players ??= new List<Player>();

            var existingPlayer = _players.SingleOrDefault(p => p.UserId == userId);

            if (existingPlayer != null)
            {
                existingPlayer.IsActive = true;
                return existingPlayer;
            }

            var player = new Player(userId, name) {IsActive = true};
            _players.Add(player);

            return player;
        }

        public void RemovePlayer(int playerId)
        {
            if (_players == null)
            {
                return;
            }

            var existingPlayer = _players.SingleOrDefault(p => p.PlayerID == playerId);

            if (existingPlayer == null)
            {
                return;
            }

            existingPlayer.IsActive = false;
        }
    }
}