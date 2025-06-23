using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum RuleScope
    {
        [Display(Name = "Cart")]
        Cart = 1,

        [Display(Name = "Customer")]
        Customer = 2,

        [Display(Name = "Recipe")]
        Recipe = 3
    }
}
