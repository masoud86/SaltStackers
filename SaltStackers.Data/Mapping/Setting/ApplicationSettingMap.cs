using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Setting
{
    public class ApplicationSettingMap : IEntityTypeConfiguration<ApplicationSetting>
    {
        public void Configure(EntityTypeBuilder<ApplicationSetting> builder)
        {
            builder.HasKey(p => p.Key);
            builder.Property(p => p.Key).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Value).HasMaxLength(int.MaxValue).IsRequired();
            builder.Property(p => p.ChangeDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();
            
            builder.HasIndex(p => p.Key);

            builder.ToTable("ApplicationSettings", Scheme.Settings);
        }
    }
}
