using System;
using Lappleken.Web.Data.Model;

namespace Lappleken.Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            var game = new Game("default");
            game.AddTeam("NW");
            game.AddTeam("AB");


            context.Games.Add(game);

            context.SaveChanges();
        }
    }
}
