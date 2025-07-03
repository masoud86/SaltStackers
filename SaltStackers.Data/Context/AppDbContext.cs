using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaltStackers.Domain.Models.Financial;
using SaltStackers.Domain.Models.Log;
using SaltStackers.Domain.Models.Membership;
using SaltStackers.Domain.Models.Message;
using SaltStackers.Domain.Models.Nutrition;
using SaltStackers.Domain.Models.Operation;
using SaltStackers.Domain.Models.Setting;

namespace SaltStackers.Data.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //Membership
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<IdentityUserRole<string>> AspNetUserRoles { get; set; }

        //Notification
        public DbSet<EmailGateway> EmailGateways { get; set; }
        public DbSet<SmsGateway> SmsGateways { get; set; }

        //Setting
        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<AlertUser> AlertUsers { get; set; }

        //Financial
        public DbSet<TaxProfile> TaxProfiles { get; set; }

        //Nutrition
        public DbSet<RecipeIngredientTypeUnit> RecipeIngredientTypeUnits { get; set; }
        public DbSet<IngredientTypeUnit> IngredientTypeUnits { get; set; }
        public DbSet<RecipeOverheadCost> RecipeOverheadCosts { get; set; }
        public DbSet<IngredientType> IngredientTypes { get; set; }
        public DbSet<FoodAttachment> FoodAttachments { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Diet> Diets { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<RecipeIngredientTypeSubstitute> RecipeIngredientTypeSubstitutes { get; set; }
        public DbSet<RecipeIngredientTypeAmount> RecipeIngredientTypeAmounts { get; set; }
        public DbSet<RecipeDiet> RecipeDiets { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<IngredientCategory> IngredientCategories { get; set; }
        public DbSet<IngredientSubCategory> IngredientSubCategories { get; set; }
        public DbSet<IngredientTypeSubCategory> IngredientTypeSubCategories { get; set; }
        public DbSet<Customization> Customizations { get; set; }
        public DbSet<RecipeOwner> RecipeOwners { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageAttachment> PackageAttachments { get; set; }
        public DbSet<PackageGroup> PackageGroups { get; set; }
        public DbSet<PackageGroupItem> PackageGroupItems { get; set; }
        public DbSet<IngredientTypeAllergenAlert> IngredientTypeAllergenAlerts { get; set; }

        //Operation
        public DbSet<OverheadCost> OverheadCosts { get; set; }
        public DbSet<Kitchen> Kitchens { get; set; }
        public DbSet<KitchenUser> KitchenUsers { get; set; }
        public DbSet<KitchenRecipe> KitchenRecipes { get; set; }

        //Log
        public DbSet<UserActivityLog> UserActivitiesLogs { get; set; }
        public DbSet<ApplicationLog> ApplicationLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes()
                .Where(e => !e.IsOwned()).SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new Mapping.Setting.ApplicationSettingMap());
            builder.ApplyConfiguration(new Mapping.Setting.ProvinceMap());
            builder.ApplyConfiguration(new Mapping.Setting.CountryMap());
            builder.ApplyConfiguration(new Mapping.Setting.ZoneMap());
            builder.ApplyConfiguration(new Mapping.Setting.CityMap());
            builder.ApplyConfiguration(new Mapping.Setting.AlertMap());
            builder.ApplyConfiguration(new Mapping.Setting.AlertUserMap());
            builder.ApplyConfiguration(new Mapping.Membership.AspNetUserMap());
            builder.ApplyConfiguration(new Mapping.Membership.AspNetRoleMap());
            builder.ApplyConfiguration(new Mapping.Message.EmailGatewayMap());
            builder.ApplyConfiguration(new Mapping.Message.SmsGatewayMap());
            builder.ApplyConfiguration(new Mapping.Financial.TaxProfileMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.RecipeIngredientTypeUnitMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.RecipeOverheadCostMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.IngredientTypeUnitMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.IngredientTypeMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.FoodAttachmentMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.IngredientMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.RecipeMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.FoodMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.UnitMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.DietMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.RecipeIngredientTypeSubstituteMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.RecipeIngredientTypeAmountMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.RecipeDietMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.ComboMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.IngredientCategoryMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.IngredientSubCategoryMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.IngredientTypeSubCategoryMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.CustomizationMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.RecipeOwnerMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.TagMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.RecipeTagMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.PackageMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.PackageAttachmentMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.PackageGroupMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.PackageGroupItemMap());
            builder.ApplyConfiguration(new Mapping.Nutrition.IngredientTypeAllergenAlertMap());
            builder.ApplyConfiguration(new Mapping.Operation.OverheadCostMap());
            builder.ApplyConfiguration(new Mapping.Operation.KitchenMap());
            builder.ApplyConfiguration(new Mapping.Operation.KitchenUserMap());
            builder.ApplyConfiguration(new Mapping.Operation.KitchenRecipeMap());
            builder.ApplyConfiguration(new Mapping.Log.UserActivitiesLogMap());
            builder.ApplyConfiguration(new Mapping.Log.ApplicationLogMap());

            builder.Seed();
        }
    }
}
