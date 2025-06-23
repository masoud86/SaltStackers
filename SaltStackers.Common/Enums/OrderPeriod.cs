using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum OrderPeriod
    {
        [Display(Name = "Manual")]
        Manual = 1,

        [Display(Name = "Weekly")]
        Weekly = 2,

        [Display(Name = "Biweekly")]
        Biweekly = 3,

        [Display(Name = "Monthly")]
        Monthly = 4
    }
}
