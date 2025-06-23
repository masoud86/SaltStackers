using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Setting
{
    public class ZoneMap : IEntityTypeConfiguration<Zone>
    {
        public void Configure(EntityTypeBuilder<Zone> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();
            builder.Property(p => p.IsActive).IsRequired();
            builder.Property(p => p.CityId).IsRequired();
            builder.Property(p => p.DeliveryPrice).IsRequired().HasDefaultValue(0.00).HasColumnType("decimal(18,2)");
            builder.Property(p => p.EditDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.ToTable("Zones", Scheme.Settings);
        }
    }
}
