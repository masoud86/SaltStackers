using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum OverheadCategory
    {
        [Display(Name = "All")]
        All = 0,

        [Display(Name = "Recipe")]
        Recipe = 1
    }
}
