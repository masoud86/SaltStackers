using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Nutrition;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Nutrition.Controllers
{
    [Area("Nutrition")]
    [BreadCrumb(Title = "Tags", UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = true, Order = 0)]
    public class TagController : Controller
    {
        private readonly INutritionService _nutritionService;

        public TagController(INutritionService nutritionService)
        {
            _nutritionService = nutritionService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 0, TitleResourceName = "Tags", TitleResourceType = typeof(Resources.Global))]
        public async Task<IActionResult> Index(TagFilters model)
        {
            return View(await _nutritionService.GetTagsModelAsync(model));
        }
    }
}
