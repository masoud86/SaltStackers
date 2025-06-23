using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition;

public class PackageGroupItemMap : IEntityTypeConfiguration<PackageGroupItem>
{
    public void Configure(EntityTypeBuilder<PackageGroupItem> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(p => p.RecipeId).IsRequired();
        builder.Property(p => p.Label).HasMaxLength(200).IsRequired();
        builder.Property(p => p.GroupId).IsRequired();
        builder.Property(p => p.CreateDateTime).HasColumnType("datetime")
            .HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd().IsRequired();

        builder.ToTable("PackageGroupItems", Scheme.Nutrition);
    }
}