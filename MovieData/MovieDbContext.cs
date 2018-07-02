using MovieData.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieData
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActorMapping> MovieActorMappings { get; set; }
        public DbSet<MovieProducerMapping> MovieProducerMappings { get; set; }
    }
}
 