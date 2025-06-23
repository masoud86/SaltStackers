namespace SaltStackers.Domain.Models.Nutrition
{
    public class IngredientCookingCategory
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public int Order { get; set; }

        public DateTime EditDateTime { get; set; }

        public List<Ingredient>? Ingredients { get; set; }
    }
}
