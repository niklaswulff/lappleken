using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Lappleken.Web.Data.Model
{
    public class Lapp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LappID { get; private set; }
        public string Text { get; private set; }
        public Player CreatedBy { get; private set; }
        public Game Game { get; }
        private List<LappLogg> _lappLoggs;
        public IReadOnlyCollection<LappLogg> LappLoggs => _lappLoggs?.ToList();

        public Lapp(Game game, Player player, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.Text = text;
            CreatedBy = player;
            Game = game;

            _lappLoggs = new List<LappLogg>();
        }

        private Lapp()
        {

        }
    }
}
