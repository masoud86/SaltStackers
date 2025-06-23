using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum RecipeSize
    {
        [Display(Name = "Default")]
        Default = 1,

        [Display(Name = "Family")]
        Family = 2
    }
}
