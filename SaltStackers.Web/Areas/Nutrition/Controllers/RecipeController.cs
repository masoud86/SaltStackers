using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Domain.Models.Nutrition;
using SaltStackers.Web.Helpers;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Nutrition.Controllers
{
    [Area("Nutrition")]
    [BreadCrumb(Title = "Nutrition", Order = 0, Url = "/Nutrition")]
    public class RecipeController : Controller
    {
        private readonly INutritionService _nutritionService;
        private readonly IMembershipService _membershipService;

        public RecipeController(INutritionService nutritionService, IMembershipService membershipService)
        {
            _nutritionService = nutritionService;
            _membershipService = membershipService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Recipes", Url = "/Nutrition/Recipe")]
        public async Task<IActionResult> Index(RecipeFilters model)
        {
            return View(await _nutritionService.GetRecipesModelAsync(model));
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipes", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Create(int? foodId)
        {
            var food = await _nutritionService.GetFoodAsync(foodId);
            if (food == null)
            {
                return NotFound();
            }
            var model = new RecipeDto
            {
                FoodId = food.Id,
                PersonalChefs = await _membershipService.GetPersonalChefAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        [Log]
        public async Task<IActionResult> Create(RecipeDto model)
        {
            ModelState.Remove("Code");
            if (ModelState.IsValid)
            {
                var createRecipe = await _nutritionService.CreateRecipeAsync(model);

                if (createRecipe.Succeeded)
                {
                    return RedirectToAction("Index", "Recipe", new { Area = "Nutrition", foodId = model.FoodId });
                }

                foreach (var error in createRecipe.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            model.PersonalChefs = await _membershipService.GetPersonalChefAsync();
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipes", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Edit(int? id)
        {
            var recipe = await _nutritionService.GetRecipeAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }
            recipe.PersonalChefs = await _membershipService.GetPersonalChefAsync();
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Edit(RecipeDto model)
        {
            ModelState.Remove("Code");
            if (model.PersonalChefId == "null")
            {
                model.PersonalChefId = null;
            }
            if (ModelState.IsValid)
            {
                var result = await _nutritionService.UpdateRecipeAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Details", "Recipe", new { Area = "Nutrition", model.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            model.PersonalChefs = await _membershipService.GetPersonalChefAsync();
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipes", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Clone(int? id)
        {
            var recipe = await _nutritionService.GetRecipeAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }
            recipe.Title = recipe.Title + " Copy";
            recipe.Foods = await _nutritionService.GetFoodsAsync(new FoodFilters
            {
                PageSize = 1000,
                Sort = "Title",
                Direction = "Asc"
            });
            return View(recipe);
        }

        [HttpPost]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, TitleResourceName = "CloneRecipe", TitleResourceType = typeof(Resources.Global))]
        public async Task<IActionResult> Clone(RecipeDto model)
        {
            ModelState.Remove("Code");
            if (ModelState.IsValid)
            {
                var result = await _nutritionService.CloneRecipeAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Recipe", new { Area = "Nutrition", foodId = model.FoodId });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipes", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Details(int? id)
        {
            var recipe = await _nutritionService.GetRecipeDetailsAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, TitleResourceName = "DeleteRecipe", TitleResourceType = typeof(Resources.Global))]
        public async Task<IActionResult> Delete(int? id)
        {
            var recipe = await _nutritionService.GetRecipeForDeleteAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [BreadCrumb(Order = 2, Title = "Recipes", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Delete(DeleteRecipe model)
        {
            if (ModelState.IsValid)
            {
                var result = await _nutritionService.DeleteRecipeAsync(model.Id);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Recipe", new { Area = "Nutrition", foodId = model.FoodId });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Recipes", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> SetDefault(int? id)
        {
            var recipe = await _nutritionService.GetRecipeAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> SetDefault(RecipeDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _nutritionService.SetDefaultRecipeAsync(model.Id);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Recipe", new { Area = "Nutrition", foodId = model.FoodId });
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
