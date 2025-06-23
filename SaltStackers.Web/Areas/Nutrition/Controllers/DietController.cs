using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Nutrition;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Nutrition.Controllers
{
    [Area("Nutrition")]
    [BreadCrumb(Title = "Nutrition", Order = 0, Url = "/Nutrition")]
    public class DietController : Controller
    {
        private readonly INutritionService _nutritionService;

        public DietController(INutritionService nutritionService)
        {
            _nutritionService = nutritionService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Diets", Url = "/Nutrition/Diet")]
        public async Task<IActionResult> Index(DietFilters model)
        {
            return View(await _nutritionService.GetDietsModelAsync(model));
        }
    }
}
