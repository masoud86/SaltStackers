using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Log;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Log
{
    public class ApplicationLogMap : IEntityTypeConfiguration<ApplicationLog>
    {
        public void Configure(EntityTypeBuilder<ApplicationLog> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.Level).HasMaxLength(200).IsRequired(false);
            builder.Property(p => p.Message).HasMaxLength(5000).IsRequired(false);
            builder.Property(p => p.Logger).HasMaxLength(200).IsRequired(false);
            builder.Property(p => p.Parameters).HasMaxLength(int.MaxValue).IsRequired(false);
            builder.Property(p => p.ReceiptNumber).HasMaxLength(50).IsRequired(false);
            builder.Property(p => p.RequestNumber).HasMaxLength(20).IsRequired(false);
            builder.Property(p => p.GroupKey).HasMaxLength(16).IsRequired(false);
            builder.Property(p => p.UserId).HasMaxLength(450).IsRequired(false);
            builder.Property(p => p.LogDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.HasIndex(p => p.ReceiptNumber);
            builder.HasIndex(p => p.RequestNumber);
            builder.HasIndex(p => p.LogDateTime);

            builder.ToTable("ApplicationLogs", Scheme.Log);
        }
    }
}
