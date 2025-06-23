using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class ChangePassword : UserLog
    {
        public string Id { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof(Resources.Security))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.PasswordPattern, ErrorMessageResourceName = "PasswordPolicy",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Security))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.PasswordPattern, ErrorMessageResourceName = "PasswordPolicy",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "NewPasswordConfirm", ResourceType = typeof(Resources.Security))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordConfirm",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string NewPasswordConfirm { get; set; }
    }

    public class ChangePasswordApi
    {
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof(Resources.Security))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Security))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must have at least 6 characters")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "NewPasswordConfirm", ResourceType = typeof(Resources.Security))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordConfirm",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string NewPasswordConfirm { get; set; }
    }
}
