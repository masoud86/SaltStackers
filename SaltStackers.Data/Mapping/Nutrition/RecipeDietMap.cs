using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class RecipeDietMap : IEntityTypeConfiguration<RecipeDiet>
    {
        public void Configure(EntityTypeBuilder<RecipeDiet> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.RecipeId).IsRequired();
            builder.Property(p => p.DietId).IsRequired();

            builder.HasKey(p => new { p.RecipeId, p.DietId });

            builder
                .HasOne<Recipe>(p => p.Recipe)
                .WithMany(p => p.RecipeDiets)
                .HasForeignKey(p => p.RecipeId);

            builder
                .HasOne<Diet>(p => p.Diet)
                .WithMany(p => p.RecipeDiets)
                .HasForeignKey(p => p.DietId);

            builder.ToTable("RecipeDiets", Scheme.Nutrition);
        }
    }
}
