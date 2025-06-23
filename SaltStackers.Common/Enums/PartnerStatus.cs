using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum PartnerStatus
    {
        [Display(Name = "Inactive")]
        Inactive = 0,

        [Display(Name = "Coming Soon")]
        ComingSoon = 1,

        [Display(Name = "Active")]
        Active = 2
    }
}
