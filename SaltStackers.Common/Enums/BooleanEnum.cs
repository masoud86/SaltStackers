using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum BooleanEnum
    {
        [Display(Name = "No Answer")]
        NoAnswer = 0,

        [Display(Name = "Yes")]
        True = 1,

        [Display(Name = "No")]
        False = 2
    }
}
