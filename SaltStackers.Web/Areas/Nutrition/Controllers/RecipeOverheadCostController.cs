using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Application.ViewModels.Operation;
using SaltStackers.Common.Enums;
using SaltStackers.Web.Helpers;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SaltStackers.Web.Areas.Nutrition.Controllers
{
    [Area("Nutrition")]
    [BreadCrumb(Title = "Nutrition", Order = 0, Url = "/Nutrition")]
    public class RecipeOverheadCostController : Controller
    {
        private readonly INutritionService _nutritionService;
        private readonly IOperationService _orderService;

        public RecipeOverheadCostController(INutritionService nutritionService, IOperationService orderService)
        {
            _nutritionService = nutritionService;
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Recipe Overhead Costs", Url = "/Nutrition/RecipeOverheadCost")]
        public async Task<IActionResult> Index(RecipeOverheadCostFilters model)
        {
            var recipe = await _nutritionService.GetRecipeAsync(model.RecipeId);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(await _nutritionService.GetRecipeOverheadCostsModelAsync(model));
        }


        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipe Overhead Costs", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Create(int? recipeId)
        {
            var recipe = await _nutritionService.GetRecipeAsync(recipeId);
            if (recipe == null)
            {
                return NotFound();
            }
            var model = new RecipeOverheadCostDto
            {
                RecipeId = recipe.Id,
                OverheadCosts = await _orderService.GetOverheadCostsAsync(new OverheadCostFilters { OverheadCategory = OverheadCategory.Recipe, PageSize = 100 })
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        [Log]
        public async Task<IActionResult> Create(RecipeOverheadCostDto model)
        {
            if (ModelState.IsValid)
            {
                var createRecipeOverheadCost = await _nutritionService.CreateRecipeOverheadCostAsync(model);

                if (createRecipeOverheadCost.Succeeded)
                {
                    return RedirectToAction("Index", "RecipeOverheadCost", new { Area = "Nutrition", recipeId = model.RecipeId });
                }

                foreach (var error in createRecipeOverheadCost.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            model.OverheadCosts = await _orderService.GetOverheadCostsAsync(new OverheadCostFilters { OverheadCategory = OverheadCategory.Recipe, PageSize = 100 });

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipe Overhead Costs", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Edit(int? id)
        {
            var recipeOverheadCost = await _nutritionService.GetRecipeOverheadCostAsync(id);

            if (recipeOverheadCost == null)
            {
                return NotFound();
            }
            recipeOverheadCost.OverheadCosts = await _orderService.GetOverheadCostsAsync(new OverheadCostFilters { OverheadCategory = OverheadCategory.Recipe, PageSize = 100 });
            return View(recipeOverheadCost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Edit(RecipeOverheadCostDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _nutritionService.UpdateRecipeOverheadCostAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "RecipeOverheadCost", new { Area = "Nutrition", recipeId = model.RecipeId });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            model.OverheadCosts = await _orderService.GetOverheadCostsAsync(new OverheadCostFilters { OverheadCategory = OverheadCategory.Recipe, PageSize = 100 });

            return View(model);
        }


        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipe Overhead Costs", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Delete(int? id)
        {
            var recipeOverheadCost = await _nutritionService.GetRecipeOverheadCostForDeleteAsync(id);

            if (recipeOverheadCost == null)
            {
                return NotFound();
            }

            return View(recipeOverheadCost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Delete(DeleteRecipeOverheadCost model)
        {
            if (ModelState.IsValid)
            {
                var result = await _nutritionService.DeleteRecipeOverheadCostAsync(model.Id);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "RecipeOverheadCost", new { Area = "Nutrition", recipeId = model.RecipeId });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
    }
}
