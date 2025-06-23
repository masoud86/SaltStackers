using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Enums;
using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class EditUser : UserLog
    {
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string? Name { get; set; }

        [Phone(ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [Display(Name = "Mobile", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.PhoneNumberPattern, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public string? Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [Display(Name = "Roles", ResourceType = typeof(Resources.Security))]
        public string? Role { get; set; }

        public List<RoleDto>? Roles { get; set; }

        public string? StripeId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "BirthDate", ResourceType = typeof(Resources.Global))]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Gender", ResourceType = typeof(Resources.Global))]
        public Gender? Gender { get; set; }

        [Display(Name = "BloodType", ResourceType = typeof(Resources.Health))]
        public BloodType? BloodType { get; set; }

        public string? Referral { get; set; }
    }
}
