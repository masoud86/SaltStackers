using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums;

public enum AllergenAlert
{
    [Display(Name = "Gluten")]
    [Icon("fas fa-wheat-awn")]
    Gluten = 1,

    [Display(Name = "Egg")]
    [Icon("fas fa-egg")]
    Egg = 2,

    [Display(Name = "Fish")]
    [Icon("fas fa-fish")]
    Fish = 3,

    [Display(Name = "Crustaceans")]
    [Icon("fas fa-shrimp")]
    Crustaceans = 4,

    [Display(Name = "Peanut")]
    [Icon("fas fa-question")]
    Peanut = 5,

    [Display(Name = "Molluscs")]
    [Icon("fas fa-question")]
    Molluscs = 6,

    [Display(Name = "Lupin")]
    [Icon("fas fa-question")]
    Lupin = 7,

    [Display(Name = "Soya")]
    [Icon("fas fa-question")]
    Soya = 8,

    [Display(Name = "Celery")]
    [Icon("fas fa-question")]
    Celery = 9,

    [Display(Name = "Milk")]
    [Icon("fas fa-cow")]
    Milk = 10,

    [Display(Name = "Tree Nuts")]
    [Icon("fas fa-question")]
    TreeNuts = 11,

    [Display(Name = "Mustard")]
    [Icon("fas fa-question")]
    Mustard = 12,

    [Display(Name = "Sesame")]
    [Icon("fas fa-question")]
    Sesame = 13,

    [Display(Name = "Sulphur Dioxide")]
    [Icon("fas fa-question")]
    SulphurDioxide = 14
}
