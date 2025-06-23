using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Operation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Operation;

public class KitchenMap : IEntityTypeConfiguration<Kitchen>
{
    public void Configure(EntityTypeBuilder<Kitchen> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(p => p.Title).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Subtitle).HasMaxLength(400).IsRequired(false);
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.ZoneId).IsRequired().HasDefaultValue(1);
        builder.Property(p => p.Logo).HasMaxLength(100).IsRequired(false);
        builder.Property(p => p.Location).HasMaxLength(200).IsRequired(false);
        builder.Property(p => p.Longitude).HasMaxLength(15).IsRequired(false);
        builder.Property(p => p.Latitude).HasMaxLength(15).IsRequired(false);
        builder.Property(p => p.PostalCode).HasMaxLength(10).IsRequired(false);
        builder.Property(p => p.PhoneNumber).HasMaxLength(15).IsRequired(false);
        builder.Property(p => p.RecipeTaxProfileId).IsRequired(false);

        builder.Property(p => p.CreateDateTime)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasIndex(p => p.Title).IsUnique();

        builder.ToTable("Kitchens", Scheme.Operation);
    }
}
