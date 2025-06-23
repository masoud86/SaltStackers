using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class RecipeIngredientTypeAmountMap : IEntityTypeConfiguration<RecipeIngredientTypeAmount>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredientTypeAmount> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.RecipeIngredientTypeUnitId).IsRequired();
            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.ProcessFee).IsRequired().HasColumnType("decimal(18,2)");

            builder.ToTable("RecipeIngredientTypeAmounts", Scheme.Nutrition);
        }
    }
}
