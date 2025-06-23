using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Api
{
    public class JwtToken
    {
        [DataType(DataType.Text)]
        [Display(Name = "Access Token")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public string AccessToken { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Refresh Token")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public string RefreshToken { get; set; }
    }
}
