using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Web.Helpers;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Nutrition.Controllers
{
    [Area("Nutrition")]
    [BreadCrumb(Title = "Nutrition", Order = 0, Url = "/Nutrition")]
    public class IngredientController : Controller
    {
        private readonly INutritionService _nutritionService;

        public IngredientController(INutritionService nutritionService)
        {
            _nutritionService = nutritionService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Ingredients", Url = "/Nutrition/Ingredient")]
        public async Task<IActionResult> Index(IngredientFilters model)
        {
            return View(await _nutritionService.GetIngredientsModelAsync(model));
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Ingredients", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Create()
        {
            return View(new IngredientDto
            {
                Units = await _nutritionService.GetUnitsAsync(new UnitFilters { PageSize = 100 })
            });
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Ingredients", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Manage(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var Ingredient = await _nutritionService.GetIngredientAsync(id);

            if (Ingredient == null)
            {
                return NotFound();
            }

            return View(Ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        [Log]
        public async Task<IActionResult> Create(IngredientDto model)
        {
            if (ModelState.IsValid)
            {
                var createIngredient = await _nutritionService.CreateIngredientAsync(model);

                if (createIngredient.Succeeded)
                {
                    return RedirectToAction("Manage", "Ingredient", new { Area = "Nutrition", id = createIngredient.Info["Id"] });
                }

                foreach (var error in createIngredient.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            model.Units = await _nutritionService.GetUnitsAsync(new UnitFilters { PageSize = 100 });

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Ingredients", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Edit(int? id)
        {
            var ingredient = await _nutritionService.GetIngredientAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            ingredient.Units = await _nutritionService.GetUnitsAsync(new UnitFilters { PageSize = 100 });


            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Edit(IngredientDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _nutritionService.UpdateIngredientAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Manage", "Ingredient", new { Area = "Nutrition", id = model.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.Units = await _nutritionService.GetUnitsAsync(new UnitFilters { PageSize = 100 });

            return View(model);
        }


        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Ingredients", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Delete(int? id)
        {
            var Ingredient = await _nutritionService.GetIngredientForDeleteAsync(id);

            if (Ingredient == null)
            {
                return NotFound();
            }

            return View(Ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Delete(DeleteIngredient model)
        {
            if (ModelState.IsValid)
            {
                var result = await _nutritionService.DeleteIngredientAsync(model.Id);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
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
