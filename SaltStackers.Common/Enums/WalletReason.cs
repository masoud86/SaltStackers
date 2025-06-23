using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum WalletReason
    {
        [Display(Name = "Unspecified")]
        Unspecified = 0,

        [Display(Name = "Administration")]
        Administration = 2,

        [Display(Name = "Purchase")]
        Purchase = 3,

        [Display(Name = "Refilling")]
        Refilling = 4,

        [Display(Name = "Refund")]
        Refund = 5,

        [Display(Name = "Partial Refund")]
        PartialRefund = 6,

        [Display(Name = "Debit")]
        Debit = 7,

        [Display(Name = "Pay Invoice")]
        PayInvoice = 8,

        [Display(Name = "Referral Gift")]
        ReferralGift = 9
    }
}
