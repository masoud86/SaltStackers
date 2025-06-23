using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Customer
{
    public class RegisterCustomer
    {
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public string Email { get; set; }

        [Phone(ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [Display(Name = "Mobile", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.PhoneNumberPattern, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Security))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public string Password { get; set; }

        public string? Referral { get; set; }
    }

    public class RegisterResult
    {
        public bool Succeeded { get; set; }

        public string ErrorMessage { get; set; }

        public string Username { get; set; }
    }
}
