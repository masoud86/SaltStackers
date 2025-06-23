using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class IngredientTypeAllergenAlertMap : IEntityTypeConfiguration<IngredientTypeAllergenAlert>
    {
        public void Configure(EntityTypeBuilder<IngredientTypeAllergenAlert> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.IngredientTypeId).IsRequired();
            builder.Property(p => p.AllergenAlert).IsRequired();

            builder.ToTable("IngredientTypeAllergenAlerts", Scheme.Nutrition);
        }
    }
}
