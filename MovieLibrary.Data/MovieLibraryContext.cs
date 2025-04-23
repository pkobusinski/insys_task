using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data.Entities;

namespace MovieLibrary.Data
{
    public class MovieLibraryContext : DbContext
    {
        public MovieLibraryContext(DbContextOptions<MovieLibraryContext> options): base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<MovieCategory> MovieCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=MovieLibrary.db");
            }
        }
    }
}
