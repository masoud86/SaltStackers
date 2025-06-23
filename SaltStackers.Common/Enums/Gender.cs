using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum Gender
    {
        [Display(Name = "Not Known")]
        NotKnown = 0,

        [Display(Name = "Male")]
        Male = 1,

        [Display(Name = "Female")]
        Female = 2,

        [Display(Name = "Not Applicable")]
        NotApplicable = 3
    }
}
