using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum RecipeChangeType
    {
        [Display(Name = "No Change")]
        NoChange = 0,

        [Display(Name = "Substitute")]
        Substitute = 1,

        [Display(Name = "Change Amount")]
        ChangeAmount = 2,

        [Display(Name = "Substitute & Change Amount")]
        SubstituteChangeAmount = 3,

        [Display(Name = "Remove Ingredient")]
        RemoveIngredient = 4,

        [Display(Name = "Add Ingredient")]
        AddIngredient = 5,

        [Display(Name = "Add Remark")]
        AddRemark = 6
    }
}
