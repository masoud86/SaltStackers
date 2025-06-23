using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class LoginCustomerVerify
    {
        [Display(Name = "Code")]
        [Required(ErrorMessage = "{0} is mandatory")]
        public string TotpCode { get; set; }
    }
}
