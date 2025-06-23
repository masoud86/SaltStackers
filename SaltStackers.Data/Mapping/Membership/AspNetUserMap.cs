using SaltStackers.Domain.Models.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Membership;

public class AspNetUserMap : IEntityTypeConfiguration<AspNetUser>
{
    public void Configure(EntityTypeBuilder<AspNetUser> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired(false);
        builder.Property(p => p.RefreshToken).HasMaxLength(int.MaxValue).IsRequired(false);
        builder.Property(p => p.RefreshTokenExpiryTime).IsRequired(false);
        builder.Property(p => p.IsBlocked).IsRequired();
        builder.Property(p => p.IsAdmin).IsRequired().HasDefaultValue(false);
        builder.Property(p => p.Referral).HasMaxLength(256).IsRequired(false);
        builder.Property(p => p.LastLogin).IsRequired(false);
        builder.Property(p => p.KitchenId).IsRequired(false);
        builder.Property(p => p.CreateDateTime)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder.Property(p => p.EditDateTime)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd()
            .IsRequired();
    }
}
