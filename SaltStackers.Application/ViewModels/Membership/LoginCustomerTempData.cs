using System;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class LoginCustomerTempData
    {
        public string SecretKey { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "MobileNumber", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression("^09[0-9]{9}$", ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string PhoneNumber { get; set; }

        public DateTime ExpirationTime { get; set; }
    }
}
