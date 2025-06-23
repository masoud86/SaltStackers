namespace SaltStackers.Domain.Models.Nutrition
{
    public class RecipeDiet
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public virtual Recipe? Recipe { get; set; }

        public int DietId { get; set; }

        public virtual Diet? Diet { get; set; }
    }
}
