using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class RecipeIngredientTypeSubstituteMap : IEntityTypeConfiguration<RecipeIngredientTypeSubstitute>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredientTypeSubstitute> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.RecipeIngredientTypeUnitId).IsRequired();
            builder.Property(p => p.IngredientTypeUnitId).IsRequired();
            builder.Property(p => p.ProcessFee).IsRequired().HasColumnType("decimal(18,2)");

            builder.ToTable("RecipeIngredientTypeSubstitutes", Scheme.Nutrition);
        }
    }
}
