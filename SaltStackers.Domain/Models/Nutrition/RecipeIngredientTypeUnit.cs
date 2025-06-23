namespace SaltStackers.Domain.Models.Nutrition
{
    public class RecipeIngredientTypeUnit
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public bool IsAddOn { get; set; }

        public bool IsDressing { get; set; }

        public int Order { get; set; }

        public int IngredientTypeUnitId { get; set; }
        public virtual IngredientTypeUnit IngredientTypeUnit { get; set; }

        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }


        public DateTime EditDateTime { get; set; }

        public virtual List<RecipeIngredientTypeSubstitute>? Substitutes { get; set; }
        public virtual List<RecipeIngredientTypeAmount>? OtherAmounts { get; set; }
    }
}
