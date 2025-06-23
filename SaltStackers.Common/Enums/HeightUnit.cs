using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum HeightUnit
    {
        [Display(Name = "Centimeter")]
        Centimeter = 1,

        [Display(Name = "Foot")]
        Foot = 2
    }
}
