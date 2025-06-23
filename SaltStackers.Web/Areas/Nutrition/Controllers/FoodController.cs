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
    public class FoodController : Controller
    {
        private readonly INutritionService _foodService;
        private readonly IUploadService _uploadService;

        public FoodController(INutritionService foodService, IUploadService uploadService)
        {
            _foodService = foodService;
            _uploadService = uploadService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Foods", Url = "/Nutrition/Food")]
        public async Task<IActionResult> Index(FoodFilters model)
        {
            return View(await _foodService.GetFoodsModelAsync(model));
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Foods", UseDefaultRouteUrl = true)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        [Log]
        public async Task<IActionResult> Create(FoodDto model)
        {
            if (ModelState.IsValid)
            {
                var createFood = await _foodService.CreateFoodAsync(model);

                if (createFood.Item1.Succeeded)
                {
                    return RedirectToAction("Edit", "Food", new { Area = "Nutrition", Id = createFood.Item2 });
                }

                foreach (var error in createFood.Item1.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Foods", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Edit(int? id)
        {
            var food = await _foodService.GetFoodAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Edit(FoodDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _foodService.UpdateFoodAsync(model);
                if (result.Succeeded)
                {
                    if (model.Uploads != null)
                    {
                        _uploadService.UploadedFiles(model.Uploads, "food", model.Id.ToString());
                    }
                    return RedirectToAction("Index");
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
        [BreadCrumb(Order = 2, Title = "Foods", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Delete(int? id)
        {
            var food = await _foodService.GetFoodForDeleteAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Delete(DeleteFood model)
        {
            if (ModelState.IsValid)
            {
                var result = await _foodService.DeleteFoodAsync(model.Id);
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
