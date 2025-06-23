using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition;

public class PackageMap : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(p => p.Code).HasMaxLength(11).IsRequired();
        builder.Property(p => p.Title).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Subtitle).HasMaxLength(200).IsRequired(false);
        builder.Property(p => p.Description).HasMaxLength(5000).IsRequired(false);
        builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(p => p.IsActive).HasDefaultValue(false).IsRequired();
        builder.Property(p => p.CreateDateTime).HasColumnType("datetime")
            .HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd().IsRequired();

        builder.ToTable("Packages", Scheme.Nutrition);
    }
}
