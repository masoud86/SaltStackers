using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum WeightUnit
    {
        [Display(Name = "Kilogram")]
        Kilogram = 1,

        [Display(Name = "Pound")]
        Pound = 2
    }
}
