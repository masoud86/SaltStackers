using SaltStackers.Data.Helper;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaltStackers.Data.Mapping.Nutrition
{
    public class RecipeMap : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(p => p.Title).HasMaxLength(200).IsRequired(false);
            builder.Property(p => p.Description).HasMaxLength(2000).IsRequired(false);
            builder.Property(p => p.RecipeDetails).HasMaxLength(int.MaxValue).IsRequired(false);
            builder.Property(p => p.FoodId).IsRequired();
            builder.Property(p => p.RecipeType).IsRequired();
            builder.Property(p => p.Skill).IsRequired();
            builder.Property(p => p.PackagingTime).IsRequired();
            builder.Property(p => p.PackagingTime).IsRequired();
            builder.Property(p => p.MainMenu).IsRequired();
            builder.Property(p => p.DefaultInCategory).IsRequired();
            builder.Property(p => p.IsOption).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.Score).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.Code).HasMaxLength(11).IsRequired();
            builder.Property(p => p.CalculateDateTime).IsRequired(false);
            builder.Property(p => p.AllowNoAppleCider).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.AllowNoPepper).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.AllowNoSalt).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.AllowNoSalmonSkin).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.StripeId).HasMaxLength(5000).IsRequired(false);
            builder.Property(p => p.HeatingInstruction).HasMaxLength(200).IsRequired(false);
            builder.Property(p => p.IsRoutine).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.Orderable).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.IsActive).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.RecipeSize).IsRequired();
            builder.Property(p => p.Priority).IsRequired(false);
            builder.Property(p => p.IsNew).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.IsTwoStepCooking).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.PersonalChefId).IsRequired(false).HasMaxLength(450);

            builder.Property(p => p.CreateDateTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            //builder.HasIndex(p => p.Code).IsUnique();

            builder.ToTable("Recipes", Scheme.Nutrition);
        }
    }
}
