using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class IngredientTypeUnitMap : IEntityTypeConfiguration<IngredientTypeUnit>
    {
        public void Configure(EntityTypeBuilder<IngredientTypeUnit> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.IngredientTypeId).IsRequired();
            builder.Property(p => p.UnitId).IsRequired();
            builder.Property(p => p.ConversionFactor).IsRequired(false);
            builder.Property(p => p.PriceOperator).HasMaxLength(1).IsRequired().HasColumnType("char");
            builder.Property(p => p.PriceFactor).IsRequired();
            builder.Property(p => p.AmountOperator).HasMaxLength(1).IsRequired(false).HasColumnType("char");
            builder.Property(p => p.AmountFactor).IsRequired(false);
            builder.Property(p => p.IsPercent).IsRequired();
            builder.Property(p => p.Energy).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Protein).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.TotalFat).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.TransFat).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.SaturatedFat).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Cholesterol).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Carbohydrate).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.DietaryFiber).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Sugars).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Sudium).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Iron).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.VitaminA).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.VitaminC).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Zinc).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Amounts).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.MakeYourOwn).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.ProfitMargin).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(p => p.EditDateTime).HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd().IsRequired();

            builder.HasOne(p => p.IngredientType).WithMany(b => b.IngredientTypeUnits)
                .HasForeignKey(p => p.IngredientTypeId);
            builder.HasOne(p => p.Unit).WithMany(b => b.IngredientTypeUnits)
                .HasForeignKey(p => p.UnitId);

            builder.ToTable("IngredientTypeUnits", Scheme.Nutrition);
        }
    }
}
