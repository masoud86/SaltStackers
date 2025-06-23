namespace SaltStackers.Domain.Models.Nutrition
{
    public class IngredientCategory
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Permalink { get; set; }

        public string? Image { get; set; }

        public int Order { get; set; }

        public DateTime EditDateTime { get; set; }

        public List<IngredientSubCategory>? IngredientSubCategories { get; set; }
    }
}
