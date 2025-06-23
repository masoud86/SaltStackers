using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum WeekDay
    {
        [Display(Name = "Sun")]
        Sunday = 0,

        [Display(Name = "Mon")]
        Monday = 1,

        [Display(Name = "Tue")]
        Tuesday = 2,

        [Display(Name = "Wed")]
        Wednesday = 3,

        [Display(Name = "Thu")]
        Thursday = 4,

        [Display(Name = "Fri")]
        Friday = 5,

        [Display(Name = "Sun")]
        Saturday = 6
    }
}
