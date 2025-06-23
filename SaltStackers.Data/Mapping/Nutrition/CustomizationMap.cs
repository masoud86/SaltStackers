using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class CustomizationMap : IEntityTypeConfiguration<Customization>
    {
        public void Configure(EntityTypeBuilder<Customization> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.RecipeId).IsRequired();
            builder.Property(p => p.UserId).HasMaxLength(450).IsRequired(false);
            builder.Property(p => p.IsDefault).IsRequired();
            builder.Property(p => p.Changes).HasMaxLength(int.MaxValue).IsRequired(false);
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)").HasDefaultValue(0.00);
            builder.Property(p => p.Energy).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.Protein).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.TotalFat).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.TransFat).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.SaturatedFat).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.Cholesterol).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.Carbohydrate).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.DietaryFiber).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.Sugars).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.Sudium).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.Iron).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.VitaminA).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.VitaminC).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);
            builder.Property(p => p.Zinc).IsRequired(false).HasColumnType("decimal(18,2)").HasDefaultValue((decimal)0);

            builder.Property(p => p.CalculateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate()
                .IsRequired();

            builder.ToTable("Customizations", Scheme.Nutrition);
        }
    }
}
