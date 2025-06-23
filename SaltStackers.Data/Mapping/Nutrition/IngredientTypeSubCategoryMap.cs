using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class IngredientTypeSubCategoryMap : IEntityTypeConfiguration<IngredientTypeSubCategory>
    {
        public void Configure(EntityTypeBuilder<IngredientTypeSubCategory> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.IngredientTypeId).IsRequired();
            builder.Property(p => p.IngredientSubCategoryId).IsRequired();
            builder.Property(p => p.CreateDateTime).HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd().IsRequired();

            builder.ToTable("IngredientTypeSubCategories", Scheme.Nutrition);
        }
    }
}
