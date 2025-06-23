using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum PartnerType
    {
        [Display(Name = "Personal Trainer")]
        PersonalTrainer = 1,

        [Display(Name = "Dietitian")]
        Dietitian = 2,

        [Display(Name = "Gym")]
        Gym = 3
    }
}
