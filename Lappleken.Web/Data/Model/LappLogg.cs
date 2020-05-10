using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lappleken.Web.Data.Model
{
    public class LappLogg
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LappLoggID { get; private set; }

        [Required]
        public Lapp Lapp {get; private set; }
        [Required]
        public Player ActionBy { get; private set;}
        [Required]
        public string Type { get; private set;}
        [Required]
        public int Phase { get; private set;}
        [Required]
        public DateTime TimeStamp { get; private set;}

        public LappLogg(Player player, string type, Lapp lapp, int phase)
        {
            ActionBy = player;
            Type = type;
            Lapp = lapp;
            Phase = phase;
            TimeStamp = DateTime.Now;
        }

        private LappLogg(){}
    }
}