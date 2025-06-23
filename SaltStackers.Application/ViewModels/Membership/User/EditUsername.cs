using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Membership.User;

public class EditUsername
{
    [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
    public required string Id { get; set; }

    [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
    [RegularExpression("^(?=[a-z0-9._]{8,20}$)(?!.*[_.]{2})[^_.].*[^_.]$", ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
    [StringLength(30,MinimumLength = 6, ErrorMessageResourceName = "StringLengthMinMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
    public required string Username { get; set; }
}
