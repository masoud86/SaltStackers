namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class PlateIngredientApi
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Unit { get; set; }

        public List<double> Amounts { get; set; }

        public List<NutritionFact> NutritionFacts { get; set; }
    }
}
