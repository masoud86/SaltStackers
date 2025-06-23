using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Message;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Message
{
    public class EmailGatewayMap : IEntityTypeConfiguration<EmailGateway>
    {
        public void Configure(EntityTypeBuilder<EmailGateway> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.From).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Display).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Host).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Username).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Password).HasMaxLength(2000).IsRequired();
            builder.Property(p => p.Port).IsRequired();
            builder.Property(p => p.EnableSsl).IsRequired();

            builder.ToTable("EmailGateways", Scheme.Message);
        }
    }
}
