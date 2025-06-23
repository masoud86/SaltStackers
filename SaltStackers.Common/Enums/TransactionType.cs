using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum TransactionType
    {
        [Display(Name = "Pay")]
        Pay = 1,

        [Display(Name = "Redeem")]
        Redeem = 2,

        [Display(Name = "Refund")]
        Refund = 3,

        [Display(Name = "Charge")]
        Charge = 4
    }
}
