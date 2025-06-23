using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class DietMap : IEntityTypeConfiguration<Diet>
    {
        public void Configure(EntityTypeBuilder<Diet> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Permalink).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Icon).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.Color).HasMaxLength(10).IsRequired(false);
            builder.Property(p => p.Description).HasMaxLength(5000).IsRequired(false);
            builder.Property(p => p.EmptyDescription).HasMaxLength(5000).IsRequired(false);
            builder.Property(p => p.Order).IsRequired();
            builder.Property(p => p.IsActive).HasDefaultValue(false).IsRequired();
            builder.Property(p => p.IsDefault).HasDefaultValue(false).IsRequired();
            builder.Property(p => p.EditDateTime).HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd().IsRequired();

            builder.ToTable("Diets", Scheme.Nutrition);
        }
    }
}
