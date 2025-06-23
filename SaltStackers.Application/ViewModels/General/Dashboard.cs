using SaltStackers.Application.ViewModels.Operation.Kitchen;

namespace SaltStackers.Application.ViewModels.General;

public class Dashboard
{
    public int KitchensCount { get; set; }

    public int FoodsCount { get; set; }

    public int RecipesCount { get; set; }

    public int CustomersCount { get; set; }

    public int IngredientsCount { get; set; }

    public int IngredientTypesCount { get; set; }
    
    public int IngredientTypeUnitsCount { get; set; }

    public List<KitchenDto>? Kitchens { get; set; }
}
