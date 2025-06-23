using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum BloodType
    {
        [Display(Name = "Not Known")]
        NotKnown = 0,

        [Display(Name = "O+")]
        O_Positive = 1,

        [Display(Name = "O-")]
        O_Negative = 2,

        [Display(Name = "A+")]
        A_Positive = 3,

        [Display(Name = "A-")]
        A_Negative = 4,

        [Display(Name = "B+")]
        B_Positive = 5,

        [Display(Name = "B-")]
        B_Negative = 6,

        [Display(Name = "AB+")]
        AB_Positive = 7,
        
        [Display(Name = "AB-")]
        AB_Negative = 8
    }
}
