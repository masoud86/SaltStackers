namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class RecipeIngredientTypeSubstituteDto
    {
        public int Id { get; set; }

        public int RecipeIngredientTypeUnitId { get; set; }
        public RecipeIngredientTypeUnitDto? RecipeIngredientTypeUnit { get; set; }

        public int IngredientTypeUnitId { get; set; }
        public IngredientTypeUnitDto? IngredientTypeUnit { get; set; }

        public decimal ProcessFee { get; set; }
    }
}
