using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Message;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Message
{
    public class SmsGatewayMap : IEntityTypeConfiguration<SmsGateway>
    {
        public void Configure(EntityTypeBuilder<SmsGateway> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.PhoneNumber).HasMaxLength(40).IsRequired();
            builder.Property(p => p.Username).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Password).HasMaxLength(2000).IsRequired();
            builder.Property(p => p.Company).IsRequired(false);

            builder.HasIndex(p => p.Name);

            builder.ToTable("SmsGateways", Scheme.Message);
        }
    }
}
