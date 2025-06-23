using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum RuleOperator
    {
        [Display(Name = "Is equal to")]
        IsEqualTo = 1,

        [Display(Name = "Is not equal to")]
        IsNotEqualTo = 2,

        [Display(Name = "Is null")]
        IsNull = 3,

        [Display(Name = "Is not null")]
        IsNotNull = 4,

        [Display(Name = "Greater than or equal to")]
        GreaterThanOrEqualTo = 5,

        [Display(Name = "Greater than")]
        GreaterThan = 6,

        [Display(Name = "Less than or equal to")]
        LessThanOrEqualTo = 7,

        [Display(Name = "Less than")]
        LessThan = 8,

        [Display(Name = "Starts with")]
        StartsWith = 9,

        [Display(Name = "Ends with")]
        EndsWith = 10,

        [Display(Name = "Contains")]
        Contains = 11,

        [Display(Name = "Not contains")]
        NotContains = 12,

        [Display(Name = "Is empty")]
        IsEmpty = 13,

        [Display(Name = "Is not empty")]
        IsNotEmpty = 14,

        [Display(Name = "In")]
        In = 15,

        [Display(Name = "Not in")]
        NotIn = 16,

        [Display(Name = "All in")]
        AllIn = 17,

        [Display(Name = "Not all in")]
        NotAllIn = 18,

        [Display(Name = "Like")]
        Like = 19,

        [Display(Name = "Not like")]
        NotLike = 20
    }
}
