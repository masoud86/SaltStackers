using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Operation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Operation
{
    public class KitchenUserMap : IEntityTypeConfiguration<KitchenUser>
    {
        public void Configure(EntityTypeBuilder<KitchenUser> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.KitchenId).IsRequired();
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.Position).HasMaxLength(200).IsRequired(false);
            builder.Property(p => p.IsOwner).IsRequired();

            builder.Property(p => p.CreateDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.ToTable("KitchenUsers", Scheme.Operation);
        }
    }
}
