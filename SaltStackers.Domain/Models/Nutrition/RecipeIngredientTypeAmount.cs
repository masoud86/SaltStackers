namespace SaltStackers.Domain.Models.Nutrition
{
    public class RecipeIngredientTypeAmount
    {
        public int Id { get; set; }

        public int RecipeIngredientTypeUnitId { get; set; }
        public virtual RecipeIngredientTypeUnit RecipeIngredientTypeUnit { get; set; }

        public decimal Amount { get; set; }

        public decimal ProcessFee { get; set; }
    }
}
