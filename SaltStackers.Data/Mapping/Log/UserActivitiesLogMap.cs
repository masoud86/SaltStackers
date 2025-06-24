using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Log;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Log
{
    public class UserActivitiesLogMap : IEntityTypeConfiguration<UserActivityLog>
    {
        public void Configure(EntityTypeBuilder<UserActivityLog> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.UserId).HasMaxLength(450).IsRequired();
            builder.Property(p => p.DescriptionResourceKey).HasMaxLength(50).IsRequired();
            builder.Property(p => p.DescriptionParameters).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.Type).HasMaxLength(100).IsRequired();
            builder.Property(p => p.ActionRelatedId).HasMaxLength(128).IsRequired(false);
            builder.Property(p => p.Content).HasMaxLength(int.MaxValue).IsRequired(false);
            builder.Property(p => p.IpAddress).HasMaxLength(16).IsRequired();
            builder.Property(p => p.Device).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.Browser).HasMaxLength(50).IsRequired(false);
            builder.Property(p => p.BrowserVersion).HasMaxLength(10).IsRequired(false);
            builder.Property(p => p.OperatingSystem).HasMaxLength(50).IsRequired(false);
            builder.Property(p => p.CreateDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.HasIndex(p => p.CreateDateTime);

            builder.ToTable("UserActivitiesLogs", Scheme.Log);
        }
    }
}
