using SaltStackers.Application.ViewModels.Nutrition;

namespace SaltStackers.Application.ViewModels.Api
{
    public class RecipeVariables
    {
        public RecipeVariables()
        {
            NutritionFacts = new List<NutritionFact>();
        }

        public decimal Price { get; set; }

        public List<NutritionFact> NutritionFacts { get; set; }

        public string? ChangeDescription { get; set; }

        public List<IngredientChangeAnalyzed>? Changes { get; set; }

        public decimal GetNutritionFact(string title)
        {
            var nutritionFact = NutritionFacts.FirstOrDefault(p => p.Title == title)?.Value;
            return nutritionFact.HasValue ? nutritionFact.Value : 0;
        }
    }
}
