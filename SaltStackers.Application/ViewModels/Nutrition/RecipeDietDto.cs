namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class RecipeDietDto
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public RecipeDto? Recipe { get; set; }

        public int DietId { get; set; }

        public DietDto? Diet { get; set; }
    }
}
