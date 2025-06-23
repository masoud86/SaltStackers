using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class RecipeOverheadCostMap : IEntityTypeConfiguration<RecipeOverheadCost>
    {
        public void Configure(EntityTypeBuilder<RecipeOverheadCost> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");

            builder.Property(p => p.EditDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.ToTable("RecipeOverheadCosts", Scheme.Nutrition);
        }
    }
}
