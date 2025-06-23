using SaltStackers.Domain.Models.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Membership
{
    public class AspNetRoleMap : IEntityTypeConfiguration<AspNetRole>
    {
        public void Configure(EntityTypeBuilder<AspNetRole> builder)
        {
            builder.Property(p => p.DisplayName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(p => p.Icon).HasMaxLength(20).IsRequired(false);
            builder.Property(p => p.IsLocked).IsRequired();
            builder.Property(p => p.CreateDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();
        }
    }
}
