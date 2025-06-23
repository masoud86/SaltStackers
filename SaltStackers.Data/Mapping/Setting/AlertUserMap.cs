using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Setting;

public class AlertUserMap : IEntityTypeConfiguration<AlertUser>
{
    public void Configure(EntityTypeBuilder<AlertUser> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(p => p.AlertId).IsRequired();
        builder.Property(p => p.UserId).HasMaxLength(450).IsRequired();
        builder.Property(p => p.IsSeen).IsRequired();
        builder.Property(p => p.ViewDateTime).HasColumnType("datetime").IsRequired(false);
        builder.Property(p => p.CreateDateTime)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.ToTable("AlertUsers", Scheme.Settings);
    }
}
