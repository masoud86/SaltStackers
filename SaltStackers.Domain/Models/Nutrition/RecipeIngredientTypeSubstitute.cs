namespace SaltStackers.Domain.Models.Nutrition
{
    public class RecipeIngredientTypeSubstitute
    {
        public int Id { get; set; }

        public int RecipeIngredientTypeUnitId { get; set; }
        public virtual RecipeIngredientTypeUnit RecipeIngredientTypeUnit { get; set; }

        public int IngredientTypeUnitId { get; set; }
        public virtual IngredientTypeUnit IngredientTypeUnit { get; set; }

        public decimal ProcessFee { get; set; }
    }
}
