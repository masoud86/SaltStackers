namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class RecipeHistory
    {
        public RecipeHistory()
        {
            NutritionFacts = new List<NutritionFact>();
        }

        public RecipeChanges? RecipeChanges { get; set; }

        public decimal Price { get; set; }

        public List<NutritionFact> NutritionFacts { get; set; }
    }
}
