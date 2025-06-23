using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum WorkoutPeriod
    {
        [Display(Name = "Daily")]
        Daily = 1,

        [Display(Name = "Weekly")]
        Weekly = 2
    }
}
