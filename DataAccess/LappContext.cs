using DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class LappContext : DbContext
    {
        public LappContext(DbContextOptions<LappContext> options) : base(options)
        {
        }

        public static string DEFAULT_SCHEMA => "dbo";
        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Lapp> Lapps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TeamEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LappEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerEntityTypeConfiguration());
        }
    }
}