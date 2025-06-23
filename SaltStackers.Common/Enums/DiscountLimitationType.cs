using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum DiscountLimitationType
    {
        [Display(Name = "Unlimited")]
        Unlimited = 0,

        [Display(Name = "N Times Only")]
        TimesOnly = 1,

        [Display(Name = "N Times Per Customer")]
        TimesPerCustomer = 3       
    }
}
