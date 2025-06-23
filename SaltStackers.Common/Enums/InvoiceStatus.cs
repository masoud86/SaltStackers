using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum InvoiceStatus
    {
        [Display(Name = "Created")]
        Created = 0,

        [Display(Name = "Pending")]
        Pending = 1,

        [Display(Name = "Unpaid")]
        Unpaid = 2,

        [Display(Name = "Paid")]
        Paid = 3,

        [Display(Name = "Cancelled")]
        Cancelled = 4
    }
}
