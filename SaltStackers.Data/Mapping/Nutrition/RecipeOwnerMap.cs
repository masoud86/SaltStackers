using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class RecipeOwnerMap : IEntityTypeConfiguration<RecipeOwner>
    {
        public void Configure(EntityTypeBuilder<RecipeOwner> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.RecipeId).IsRequired();
            builder.Property(p => p.UserId).HasMaxLength(450).IsRequired();
            
            builder.Property(p => p.CreateDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.ToTable("RecipeOwners", Scheme.Nutrition);
        }
    }
}
