using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum SkillLevel
    {
        [Display(Name = "Easy")]
        Easy = 1,

        [Display(Name = "Moderate")]
        Moderate = 2,

        [Display(Name = "Hard")]
        Hard = 3
    }
}
