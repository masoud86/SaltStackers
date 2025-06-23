using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class FoodAttachmentMap : IEntityTypeConfiguration<FoodAttachment>
    {
        public void Configure(EntityTypeBuilder<FoodAttachment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.FileName).HasMaxLength(200).IsRequired();
            builder.Property(p => p.IsMain).IsRequired();
            builder.Property(p => p.MediaType).IsRequired();
            builder.Property(p => p.FoodId).IsRequired();

            builder.Property(p => p.UploadDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.ToTable("FoodAttachments", Scheme.Nutrition);
        }
    }
}
