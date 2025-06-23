using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class IngredientCookingCategoryMap : IEntityTypeConfiguration<IngredientCookingCategory>
    {
        public void Configure(EntityTypeBuilder<IngredientCookingCategory> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Order).IsRequired();
            builder.Property(p => p.EditDateTime).HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd().IsRequired();

            builder.ToTable("IngredientCookingCategories", Scheme.Nutrition);
        }
    }
}
