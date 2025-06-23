using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum StressLevel
    {
        [Display(Name = "Low")]
        Low = 1,

        [Display(Name = "Average")]
        Average = 2,

        [Display(Name = "High")]
        High = 3
    }
}
