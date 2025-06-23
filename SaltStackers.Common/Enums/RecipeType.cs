using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum RecipeType
    {
        [Display(Name = "Personal Chef")]
        PersonalChef = 1,

        [Display(Name = "MealPrep")]
        MealPrep = 2
    }
}
