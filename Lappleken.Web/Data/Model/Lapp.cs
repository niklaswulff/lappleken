﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [MinLength(1)]
        public string Text { get; private set; }
        [Required]
        public Player CreatedBy { get; private set; }
        [Required]
        public Game Game { get; private set; }
        public Game.PhaseEnum? ClaimedInPhase { get; private set; }
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

        public void AddLogg(Game.PhaseEnum phaseId, Player playerId, string action)
        {
            if (action == "claim")
            {
                ClaimedInPhase = phaseId;
            }

            _lappLoggs ??= new List<LappLogg>();

            _lappLoggs.Add(new LappLogg(playerId, action, this, (int)phaseId));
        }
    }
}
