using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums;

public enum AlertType
{
    [Display(Name = "Info")]
    Info = 1,

    [Display(Name = "Warning")]
    Warning = 2,

    [Display(Name = "Danger")]
    Danger = 3,

    [Display(Name = "Success")]
    Success = 4
}
