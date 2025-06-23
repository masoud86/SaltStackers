using System.ComponentModel.DataAnnotations;
namespace SaltStackers.Common.Enums;

public enum OrderStatusType
{
    [Display(Name = "Processing")]
    Processing = 0,

    [Display(Name = "Cooked")]
    Cooked = 1,

    [Display(Name = "Shipped")]
    Shipped = 2,

    [Display(Name = "Completed")]
    Completed = 3,

    [Display(Name = "Canceled")]
    Canceled = 4
}