namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class IngredientCookingCategoryDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public int Order { get; set; }

        public DateTime EditDateTime { get; set; }

        public List<IngredientDto>? Ingredients { get; set; }
    }
}
