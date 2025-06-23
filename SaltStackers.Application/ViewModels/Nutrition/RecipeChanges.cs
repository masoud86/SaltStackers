using SaltStackers.Common.Enums;
using SaltStackers.Common.Helper;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class RecipeChanges
    {
        public RecipeChanges()
        {
            IngredientChanges = new List<IngredientChange>();
            RecipeAddOns = new List<RecipeAddOn>();
            RecipeFlags = new List<RecipeFlag>();
            Combos = new List<string>();
        }

        public List<IngredientChange>? IngredientChanges { get; set; }

        public List<RecipeAddOn>? RecipeAddOns { get; set; }

        public List<RecipeFlag>? RecipeFlags { get; set; }

        public List<string>? Combos { get; set; }
    }

    public class IngredientChange
    {
        public int Id { get; set; }

        public int? SubstituteId { get; set; }

        public float Size { get; set; }

        public string? Unit { get; set; }
    }

    public class IngredientChangeAnalyzed : IngredientChange
    {
        public string? Title { get; set; }

        public string? SubstituteTitle { get; set; }

        public decimal? DefaultSize { get; set; }

        public RecipeChangeType ChangeType { get; set; }

        public string? ChangeTypeTitle => EnumHelper<RecipeChangeType>.GetDisplayValue(ChangeType);

        public string? Description { get; set; }
    }

    public class RecipeAddOn
    {
        public int Id { get; set; }

        public float Size { get; set; }
    }

    public class RecipeAddOnAnalyzed : RecipeAddOn
    {
        public string Title { get; set; }
    }

    public class RecipeFlag
    {
        public string Key { get; set; }

        public bool Value { get; set; }
    }

    public class RecipeCombo
    {
        public string Code { get; set; }

        public string Title { get; set; }
    }
}
