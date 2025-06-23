namespace SaltStackers.Application.ViewModels.Nutrition;

public class RecipeDetails
{
    public int RecipeId { get; set; }

    public int FoodId { get; set; }

    public string Title { get; set; }

    public List<string> Images { get; set; }

    public bool MainSize { get; set; }

    public bool IsDefault { get; set; }
}
