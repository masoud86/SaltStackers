using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class LoginCustomer
    {
        [Phone(ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [Display(Name = "Mobile", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.PhoneNumberPattern, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string PhoneNumber { get; set; }
    }
}
