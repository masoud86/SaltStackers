using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Financial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Financial;

public class TaxProfileMap : IEntityTypeConfiguration<TaxProfile>
{
    public void Configure(EntityTypeBuilder<TaxProfile> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(p => p.Title).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(2000).IsRequired(false);
        builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(p => p.CreateDateTime).HasColumnType("datetime")
            .HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd().IsRequired();

        builder.ToTable("TaxProfiles", Scheme.Financial);
    }
}
