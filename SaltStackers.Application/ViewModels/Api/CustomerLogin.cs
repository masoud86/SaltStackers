using SaltStackers.Application.ViewModels.Customer;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Api;
public class CustomerLogin
{
    [DataType(DataType.Text)]
    [Display(Name = "Username", ResourceType = typeof(Resources.Security))]
    [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
    public string Username { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Password", ResourceType = typeof(Resources.Security))]
    [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
    public string Password { get; set; }
}

public class LoginResult : JwtToken
{
    public CustomerInformation? CustomerInformation { get; set; }
}