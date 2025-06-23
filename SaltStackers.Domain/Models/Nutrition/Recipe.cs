using SaltStackers.Common.Enums;
using SaltStackers.Domain.Models.Membership;

namespace SaltStackers.Domain.Models.Nutrition
{
    public class Recipe
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? RecipeDetails { get; set; }

        public int FoodId { get; set; }
        public virtual Food? Food { get; set; }

        public RecipeType RecipeType { get; set; }

        public SkillLevel Skill { get; set; }

        public int PackagingTime { get; set; }

        public bool MainMenu { get; set; }

        public bool DefaultInCategory { get; set; }

        public bool IsOption { get; set; }

        public decimal Price { get; set; }

        public decimal Score { get; set; }

        public string Code { get; set; }

        public DateTime CreateDateTime { get; set; }
        
        public DateTime? CalculateDateTime { get; set; }

        public bool AllowNoAppleCider { get; set; }
        
        public bool AllowNoPepper { get; set; }
        
        public bool AllowNoSalt { get; set; }

        public bool AllowNoSalmonSkin { get; set; }

        public string? StripeId { get; set; }

        public string? HeatingInstruction { get; set; }

        public bool IsActive { get; set; }
        
        public bool IsRoutine { get; set; }
        
        public bool Orderable { get; set; }

        public int? Priority { get; set; }

        public bool IsNew { get; set; }

        public bool IsTwoStepCooking { get; set; }

        public RecipeSize RecipeSize { get; set; } = RecipeSize.Default;

        public string? PersonalChefId { get; set; }

        public virtual AspNetUser? PersonalChef { get; set; }

        public virtual List<RecipeOverheadCost>? RecipeOverheadCosts { get; set; }

        public virtual List<RecipeIngredientTypeUnit>? RecipeIngredientTypeUnits { get; set; }

        public virtual List<RecipeDiet>? RecipeDiets { get; set; }
        
        public virtual List<RecipeTag>? RecipeTags { get; set; }

        public virtual List<Customization>? Customizations { get; set; }

        public virtual List<RecipeOwner>? RecipeOwners { get; set; }
    }
}
