using SaltStackers.Common.Enums;

namespace SaltStackers.Application.ViewModels.Nutrition;

public class IngredientTypeAllergenAlertDto
{
    public int Id { get; set; }

    public int IngredientTypeId { get; set; }

    public IngredientTypeDto? IngredientType { get; set; }

    public AllergenAlert AllergenAlert { get; set; }
}
