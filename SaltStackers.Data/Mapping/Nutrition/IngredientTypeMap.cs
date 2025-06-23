using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class IngredientTypeMap : IEntityTypeConfiguration<IngredientType>
    {
        public void Configure(EntityTypeBuilder<IngredientType> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.Title).HasMaxLength(200).IsRequired();
            builder.Property(p => p.DisplayTitle).HasMaxLength(200).IsRequired();
            builder.Property(p => p.BasePrice).IsRequired().HasColumnType("decimal(18,4)").HasDefaultValue(1.00);
            builder.Property(p => p.MixDescription).HasMaxLength(500).IsRequired(false).HasDefaultValue(null);
            builder.Property(p => p.Pchef).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.NeedsPrep).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.IngredientId).IsRequired();
            builder.Property(p => p.EditDateTime).HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd().IsRequired();

            builder.ToTable("IngredientTypes", Scheme.Nutrition);
        }
    }
}
