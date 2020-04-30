using System;
using DataAccess.Model;

namespace DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(LappContext context)
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
