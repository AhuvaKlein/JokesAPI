using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JokesApi.Data
{
    public class JokeContext : DbContext
    {
        private string _connectionString { get; set; }

        public JokeContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Joke> Jokes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.UserId, l.JokeId });

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Joke)
                .WithMany(j => j.Likes)
                .HasForeignKey(l => l.JokeId);
        }


    }

}
