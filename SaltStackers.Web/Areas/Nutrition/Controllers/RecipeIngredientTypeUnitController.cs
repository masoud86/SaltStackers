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
    public class RecipeIngredientTypeUnitController : Controller
    {
        private readonly INutritionService _nutritionService;

        public RecipeIngredientTypeUnitController(INutritionService nutritionService)
        {
            _nutritionService = nutritionService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Recipe Units", Url = "/Nutrition/RecipeIngredientTypeUnit")]
        public async Task<IActionResult> Index(RecipeIngredientTypeUnitFilters model)
        {
            return View(await _nutritionService.GetRecipeIngredientTypeUnitsModelAsync(model));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetIngredientTypes(int ingredientId)
        {
            var ingredientTypes = await _nutritionService.GetIngredientTypesAsync(new IngredientTypeFilters
            {
                PageSize = 1000,
                Sort = "Title",
                Direction = "ASC",
                IngredientId = ingredientId
            });
            var model = new IngredientTypeDropDownDto
            {
                IngredientTypes = ingredientTypes
            };
            return PartialView("IngredientType", model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetIngredientTypeUnits(int ingredientTypeId)
        {
            var ingredientTypeunits = await _nutritionService.GetIngredientTypeUnitsAsync(new IngredientTypeUnitFilters
            {
                PageSize = 1000,
                IngredientTypeId = ingredientTypeId
            });
            var model = new IngredientTypeUnitDropDownDto
            {
                IngredientTypeUnits = ingredientTypeunits
            };
            return PartialView("IngredientTypeUnit", model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipe Units", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Create(int? recipeId)
        {
            var recipe = await _nutritionService.GetRecipeAsync(recipeId);
            if (recipe == null)
            {
                return NotFound();
            }
            var model = new RecipeIngredientTypeUnitDto
            {
                RecipeId = recipe.Id,
                Ingredients = await _nutritionService.GetIngredientsAsync(new IngredientFilters
                {
                    PageSize = 1000,
                    Sort = "Title",
                    Direction = "ASC"
                })
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        [Log]
        public async Task<IActionResult> Create(RecipeIngredientTypeUnitDto model)
        {
            if (ModelState.IsValid)
            {
                var createRecipeIngredientTypeUnit = await _nutritionService.CreateRecipeIngredientTypeUnitAsync(model);

                if (createRecipeIngredientTypeUnit.Succeeded)
                {
                    return RedirectToAction("Index", "RecipeIngredientTypeUnit", new { Area = "Nutrition", recipeId = model.RecipeId });
                }

                foreach (var error in createRecipeIngredientTypeUnit.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            model.Ingredients = await _nutritionService.GetIngredientsAsync(new IngredientFilters
            {
                PageSize = 1000,
                Sort = "Title",
                Direction = "ASC"
            });
            model.IngredientTypes = await _nutritionService.GetIngredientTypesAsync(new IngredientTypeFilters
            {
                PageSize = 1000,
                Direction = "ASC",
                IngredientId = model.IngredientId
            });
            model.IngredientTypeUnits = await _nutritionService.GetIngredientTypeUnitsAsync(new IngredientTypeUnitFilters
            {
                PageSize = 1000,
                Direction = "ASC",
                IngredientTypeId = model.IngredientTypeId
            });

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipe Units", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Edit(int? id)
        {
            var recipeIngredientTypeUnit = await _nutritionService.GetRecipeIngredientTypeUnitAsync(id);

            if (recipeIngredientTypeUnit == null)
            {
                return NotFound();
            }
            recipeIngredientTypeUnit.Ingredients = await _nutritionService.GetIngredientsAsync(new IngredientFilters
            {
                PageSize = 1000,
                Sort = "Title",
                Direction = "ASC"
            });
            recipeIngredientTypeUnit.IngredientTypes = await _nutritionService.GetIngredientTypesAsync(new IngredientTypeFilters
            {
                PageSize = 1000,
                IngredientId = recipeIngredientTypeUnit.IngredientId
            });
            recipeIngredientTypeUnit.IngredientTypeUnits = await _nutritionService.GetIngredientTypeUnitsAsync(new IngredientTypeUnitFilters
            {
                PageSize = 1000,
                IngredientTypeId = recipeIngredientTypeUnit.IngredientTypeId
            });
            return View(recipeIngredientTypeUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Edit(RecipeIngredientTypeUnitDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _nutritionService.UpdateRecipeIngredientTypeUnitAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "RecipeIngredientTypeUnit", new { Area = "Nutrition", recipeId = model.RecipeId });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            model.Ingredients = await _nutritionService.GetIngredientsAsync(new IngredientFilters
            {
                PageSize = 1000,
                Sort = "Title",
                Direction = "ASC"
            });
            model.IngredientTypes = await _nutritionService.GetIngredientTypesAsync(new IngredientTypeFilters
            {
                PageSize = 1000,
                IngredientId = model.IngredientId
            });
            model.IngredientTypeUnits = await _nutritionService.GetIngredientTypeUnitsAsync(new IngredientTypeUnitFilters
            {
                PageSize = 1000,
                IngredientTypeId = model.IngredientTypeId
            });

            return View(model);
        }


        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipe Units", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Delete(int? id)
        {
            var RecipeIngredientTypeUnit = await _nutritionService.GetRecipeIngredientTypeUnitForDeleteAsync(id);

            if (RecipeIngredientTypeUnit == null)
            {
                return NotFound();
            }

            return View(RecipeIngredientTypeUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Delete(DeleteRecipeIngredientTypeUnit model)
        {
            ModelState.Remove("Title");
            if (ModelState.IsValid)
            {
                var result = await _nutritionService.DeleteRecipeIngredientTypeUnitAsync(model.Id);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "RecipeIngredientTypeUnit", new { Area = "Nutrition", recipeId = model.RecipeId });
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
