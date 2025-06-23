using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum ShippingMethod
    {
        [Display(Name = "Delivery")]
        Delivery = 1,

        [Display(Name = "Pickup")]
        Pickup = 2
    }
}
