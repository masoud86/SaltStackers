using SaltStackers.Application.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition.Package;

public class PackageFilters : Pagination
{
    public PackageFilters() : base("Title")
    {
    }

    [Display(Name = "Actives")]
    public bool OnlyActives { get; set; } = true;
}