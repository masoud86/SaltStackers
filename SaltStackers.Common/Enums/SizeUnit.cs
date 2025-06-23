using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum SizeUnit
    {
        [Display(Name = "Centimeter")]
        Centimeter = 1,

        [Display(Name = "Inch")]
        Inch = 2
    }
}
