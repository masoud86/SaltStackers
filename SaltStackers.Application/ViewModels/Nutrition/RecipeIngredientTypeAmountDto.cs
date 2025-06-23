namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class RecipeIngredientTypeAmountDto
    {
        public int Id { get; set; }

        public int RecipeIngredientTypeUnitId { get; set; }
        public RecipeIngredientTypeUnitDto? RecipeIngredientTypeUnit { get; set; }

        public decimal Amount { get; set; }

        public decimal ProcessFee { get; set; }
    }
}
