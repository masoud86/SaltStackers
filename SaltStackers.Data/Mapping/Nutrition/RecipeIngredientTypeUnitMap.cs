using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class RecipeIngredientTypeUnitMap : IEntityTypeConfiguration<RecipeIngredientTypeUnit>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredientTypeUnit> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.IsAddOn).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.IsDressing).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.Order).IsRequired().HasDefaultValue(1);

            builder.Property(p => p.EditDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.ToTable("RecipeIngredientTypeUnits", Scheme.Nutrition);
        }
    }
}
