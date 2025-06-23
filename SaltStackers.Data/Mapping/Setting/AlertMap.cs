using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Setting;

public class AlertMap : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(p => p.Title).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Body).HasMaxLength(int.MaxValue).IsRequired();
        builder.Property(p => p.Image).HasMaxLength(100).IsRequired(false);
        builder.Property(p => p.StartDateTime).HasColumnType("datetime").IsRequired(false);
        builder.Property(p => p.EndDateTime).HasColumnType("datetime").IsRequired(false);
        builder.Property(p => p.IsPublic).IsRequired();
        builder.Property(p => p.NeedTracking).IsRequired();
        builder.Property(p => p.IsDismissable).IsRequired();
        builder.Property(p => p.IsActive).IsRequired();
        builder.Property(p => p.Type).IsRequired();
        builder.Property(p => p.CreateDateTime)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.ToTable("Alerts", Scheme.Settings);
    }
}
