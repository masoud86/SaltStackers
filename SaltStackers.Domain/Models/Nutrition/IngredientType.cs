namespace SaltStackers.Domain.Models.Nutrition
{
    public class IngredientType
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string DisplayTitle { get; set; }

        public decimal BasePrice { get; set; }

        public string? MixDescription { get; set; }

        public bool Pchef { get; set; }
        
        public bool NeedsPrep { get; set; }

        public int IngredientId { get; set; }
        public virtual Ingredient? Ingredient { get; set; }

        public DateTime EditDateTime { get; set; }

        public virtual List<IngredientTypeUnit>? IngredientTypeUnits { get; set; }
        public virtual List<IngredientTypeSubCategory>? IngredientTypeSubCategories { get; set; }
        public virtual List<IngredientTypeAllergenAlert>? AllergenAlerts { get; set; }
    }
}
