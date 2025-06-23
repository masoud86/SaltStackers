using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Nutrition.Package;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Nutrition.Controllers
{
    [Area("Nutrition")]
    [BreadCrumb(Title = "Nutrition", Order = 0, Url = "/Nutrition")]
    public class PackageController : Controller
    {
        private readonly INutritionService _nutritionService;

        public PackageController(INutritionService nutritionService)
        {
            _nutritionService = nutritionService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Packages", Url = "/Nutrition/Package")]
        public async Task<IActionResult> Index(PackageFilters model)
        {
            model.OnlyActives = false;
            return View(await _nutritionService.GetPackagesModelAsync(model));
        }

        [HttpPost]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Create(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var create = await _nutritionService.CreatePackageAsync(title);
                if (create.succeeded)
                {
                    return RedirectToAction("Edit", "Package", new { Area = "Nutrition", Id = create.id });
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Packages", UseDefaultRouteUrl = true)]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            return View(id);
        }
    }
}
