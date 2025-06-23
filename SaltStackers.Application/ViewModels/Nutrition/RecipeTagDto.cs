namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class RecipeTagDto
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public RecipeDto? Recipe { get; set; }

        public int TagId { get; set; }

        public TagDto? Tag { get; set; }
    }
}
