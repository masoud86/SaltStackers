using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Customer
{
    public class CustomerProfileDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public UserDto User { get; set; }

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "BirthDate", ResourceType = typeof(Resources.Global))]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Gender", ResourceType = typeof(Resources.Global))]
        public Gender? Gender { get; set; }

        [Display(Name = "BloodType", ResourceType = typeof(Resources.Health))]
        public BloodType? BloodType { get; set; }
    }

    public class CustomerProfileApi
    {
        public string? Name { get; set; }

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "BirthDate", ResourceType = typeof(Resources.Global))]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Gender", ResourceType = typeof(Resources.Global))]
        public Gender? Gender { get; set; }

        [Display(Name = "BloodType", ResourceType = typeof(Resources.Health))]
        public BloodType? BloodType { get; set; }

        public string? Referral { get; set; }

        public decimal Wallet { get; set; }
    }
}
