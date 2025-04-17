using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieLibrary.Data.Entities.EFMapping
{
    internal class MovieCategoryMap : IEntityTypeConfiguration<MovieCategory>
    {
        public void Configure(EntityTypeBuilder<MovieCategory> builder)
        {
            builder.ToTable("MovieCategories", "MovieLibrary");
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Movie)
                .WithMany(e => e.MovieCategories)
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Category)
                .WithMany(e => e.MovieCategories)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
