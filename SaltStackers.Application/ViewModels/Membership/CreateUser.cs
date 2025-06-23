using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class CreateUser : UserLog
    {
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string? Name { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Security))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.PasswordPattern, ErrorMessageResourceName = "PasswordPolicy",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirm", ResourceType = typeof(Resources.Security))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [Compare("Password", ErrorMessageResourceName = "PasswordConfirm",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string PasswordConfirm { get; set; }

        [Phone(ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [Display(Name = "Mobile", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.PhoneNumberPattern, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [Display(Name = "Roles", ResourceType = typeof(Resources.Security))]
        public string Role { get; set; }

        public List<RoleDto> Roles { get; set; }
    }
}
