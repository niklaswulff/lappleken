using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess
{
    public class GameEntityTypeConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> gameConfiguration)
        {
            gameConfiguration.ToTable("Game", LappContext.DEFAULT_SCHEMA);
            // Other configuration

            var teamNavigation = gameConfiguration.Metadata.FindNavigation(nameof(Game.Teams));

            //EF access the OrderItem collection property through its backing field
            teamNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var lappNavigation = gameConfiguration.Metadata.FindNavigation(nameof(Game.Lapps));

            //EF access the OrderItem collection property through its backing field
            lappNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            // Other configuration
        }
    }
    public class TeamEntityTypeConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> teamConfiguration)
        {
            teamConfiguration.ToTable("Team", LappContext.DEFAULT_SCHEMA);
            // Other configuration

            var playerNavigation = teamConfiguration.Metadata.FindNavigation(nameof(Team.Players));

            //EF access the OrderItem collection property through its backing field
            playerNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
    public class LappEntityTypeConfiguration : IEntityTypeConfiguration<Lapp>
    {
        public void Configure(EntityTypeBuilder<Lapp> lappConfiguration)
        {
            lappConfiguration.ToTable("Lapp", LappContext.DEFAULT_SCHEMA);
            // Other configuration
        }
    }
    public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> playerConfiguration)
        {
            playerConfiguration.ToTable("Player", LappContext.DEFAULT_SCHEMA);
        }
    }
}