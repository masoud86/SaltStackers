using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum DiscountType
    {
        [Display(Name = "Invoice Subtotal")]
        AssignedToInvoiceSubtotal = 1,

        [Display(Name = "Shipping")]
        AssignedToShipping = 2

        //[Display(Name = "Products")]
        //AssignedToProducts = 3
    }
}
