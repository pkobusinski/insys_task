using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieLibrary.Data.Entities.EFMapping
{
    internal class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories", "MovieLibrary");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(150);

            builder.HasMany(e => e.MovieCategories)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
