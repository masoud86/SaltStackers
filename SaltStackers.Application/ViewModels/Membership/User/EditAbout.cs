using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Membership.User;

public class EditAbout
{
    [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
    public required string Id { get; set; }

    public string? About { get; set; }
}
