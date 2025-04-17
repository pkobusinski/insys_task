using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieLibrary.Data.Entities.EFMapping
{
    internal class MovieMap : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies", "MovieLibrary");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .HasMaxLength(150);
            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.HasMany(e => e.MovieCategories)
                .WithOne(e => e.Movie)
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
