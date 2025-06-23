using SaltStackers.Application.ViewModels.Membership;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class CustomizationDto
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public RecipeDto? Recipe { get; set; }

        public string? UserId { get; set; }
        public UserDto? User { get; set; }

        public bool IsDefault { get; set; }

        public string? Changes { get; set; }

        public decimal Price { get; set; }

        public decimal? Energy { get; set; }

        public decimal? Protein { get; set; }

        public decimal? TotalFat { get; set; }

        public decimal? TransFat { get; set; }

        public decimal? SaturatedFat { get; set; }

        public decimal? Cholesterol { get; set; }

        public decimal? Carbohydrate { get; set; }

        public decimal? DietaryFiber { get; set; }

        public decimal? Sugars { get; set; }

        public decimal? Sudium { get; set; }

        public decimal? Iron { get; set; }

        public decimal? VitaminA { get; set; }

        public decimal? VitaminC { get; set; }

        public decimal? Zinc { get; set; }

        public DateTime CalculateTime { get; set; }
    }
}
