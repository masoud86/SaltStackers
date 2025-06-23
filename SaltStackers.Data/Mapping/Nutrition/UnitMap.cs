using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class UnitMap : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.Category).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Title).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Sign).HasMaxLength(10).IsRequired(false);
            builder.Property(p => p.ConversionFactor).IsRequired(false);
            builder.Property(p => p.HasCustomConversionFactor).IsRequired();


            builder.ToTable("Units", Scheme.Nutrition);
        }
    }
}
