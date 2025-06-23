using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum PaymentGateway
    {
        [Display(Name = "e-Transfer")]
        Etransfer = 0,

        [Display(Name = "Stripe")]
        Stripe = 1,

        [Display(Name = "Wallet")]
        Wallet = 2,

        [Display(Name = "Square")]
        Square = 3
    }
}
