using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Application.ViewModels.Nutrition.Package;
using SaltStackers.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SaltStackers.Web.Areas.Nutrition.Controllers
{
    [ApiController]
    public class ApiNutritionController : ControllerBase
    {
        private readonly INutritionService _nutritionService;

        public ApiNutritionController(INutritionService nutritionService)
        {
            _nutritionService = nutritionService;
        }

        private static List<ServiceError> ModelStateToServiceError(ModelStateDictionary modelstate)
        {
            var errors = new List<ServiceError>();
            foreach (var error in modelstate.Values.SelectMany(v => v.Errors))
            {
                errors.Add(new ServiceError
                {
                    Level = ErrorLevel.Blocker,
                    Description = error.ErrorMessage
                });
            }
            return errors;
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetIngredientTypes")]
        public async Task<IActionResult> GetIngredientTypes(int ingredientId)
        {
            var model = await _nutritionService.GetIngredientTypesAsync(new IngredientTypeFilters
            {
                PageSize = 100,
                IngredientId = ingredientId
            });
            return Ok(model);
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/CreateIngredientType")]
        public async Task<IActionResult> CreateIngredientType(IngredientTypeDto model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var createIngredientType = await _nutritionService.CreateIngredientTypeAsync(model);

                if (createIngredientType.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in createIngredientType.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return Ok(new ServiceResult(false, errors));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/EditIngredientType")]
        public async Task<IActionResult> EditIngredientType(IngredientTypeDto model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var createIngredientType = await _nutritionService.UpdateIngredientTypeAsync(model);

                if (createIngredientType.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in createIngredientType.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return Ok(new ServiceResult(false, errors));
        }

        [Log]
        [Authorize]
        [HttpDelete("Api/Nutrition/DeleteIngredientType")]
        public async Task<IActionResult> DeleteIngredientType(int ingredientTypeId)
        {
            var result = await _nutritionService.DeleteIngredientTypeAsync(ingredientTypeId);
            if (result.Succeeded)
            {
                return Ok(new ServiceResult(true));
            }

            return Ok(new ServiceResult(false, result.Errors));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetAllUnits")]
        public async Task<IActionResult> GetAllUnits()
        {
            return Ok(await _nutritionService.GetUnitsAsync(new UnitFilters
            {
                PageSize = 10000,
                Direction = "ASC"
            }));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/CreateIngredientTypeUnit")]
        public async Task<IActionResult> CreateIngredientTypeUnit(IngredientTypeUnitDto model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var createIngredientTypeUnit = await _nutritionService.CreateIngredientTypeUnitAsync(model);

                if (createIngredientTypeUnit.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in createIngredientTypeUnit.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return Ok(new ServiceResult(false, errors));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/EditIngredientTypeUnit")]
        public async Task<IActionResult> EditIngredientTypeUnit(IngredientTypeUnitDto model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var createIngredientType = await _nutritionService.UpdateIngredientTypeUnitAsync(model);

                if (createIngredientType.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in createIngredientType.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return Ok(new ServiceResult(false, errors));
        }

        [Log]
        [Authorize]
        [HttpDelete("Api/Nutrition/DeleteIngredientTypeUnit")]
        public async Task<IActionResult> DeleteIngredientTypeUnit(int ingredientTypeUnitId)
        {
            var result = await _nutritionService.DeleteIngredientTypeUnitAsync(ingredientTypeUnitId);
            if (result.Succeeded)
            {
                return Ok(new ServiceResult(true));
            }

            return Ok(new ServiceResult(false, result.Errors));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/SearchRecipes")]
        public async Task<IActionResult> SearchRecipes(string query, int? kitchenId = null)
        {
            return Ok(await _nutritionService.GetRecipesAsync(new RecipeFilters
            {
                PageSize = 20,
                Sort = "Title",
                Direction = "ASC",
                Query = query,
                OnlyActives = true,
                KitchenId = kitchenId
            }));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetRecipeDetails")]
        public async Task<IActionResult> GetRecipeDetails(int id)
        {
            return Ok(await _nutritionService.GetRecipeAsync(id));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetRecipeIngredients")]
        public async Task<IActionResult> GetRecipeIngredients(int id)
        {
            return Ok(await _nutritionService.GetRecipeIngredientTypeUnitsAsync(new RecipeIngredientTypeUnitFilters
            {
                RecipeId = id,
                PageSize = 100,
                Sort = "IsAddOn,Amount"
            }));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetRecipeOverheads")]
        public async Task<IActionResult> GetRecipeOverheads(int id)
        {
            return Ok(await _nutritionService.GetRecipeOverheadCostsAsync(new RecipeOverheadCostFilters
            {
                RecipeId = id,
                PageSize = 1000
            }));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetNutritionFacts")]
        public async Task<IActionResult> GetNutritionFacts(int id)
        {
            return Ok(await _nutritionService.GetNutritionFactsAsync(id));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetIngredientOtherAmounts")]
        public async Task<IActionResult> GetIngredientOtherAmounts(int id)
        {
            return Ok(await _nutritionService.GetIngredientOtherAmountsAsync(id));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/AddNewIngredientOtherAmount")]
        public async Task<IActionResult> AddNewIngredientOtherAmount(RecipeIngredientTypeAmountDto model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var createIngredientType = await _nutritionService.CreateRecipeIngredientTypeAmountAsync(model);

                if (createIngredientType.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in createIngredientType.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return Ok(new ServiceResult(false, errors));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/EditIngredientOtherAmount")]
        public async Task<IActionResult> EditIngredientOtherAmount(RecipeIngredientTypeAmountDto model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var createIngredientType = await _nutritionService.EditRecipeIngredientTypeAmountAsync(model);

                if (createIngredientType.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in createIngredientType.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return Ok(new ServiceResult(false, errors));
        }

        [Log]
        [Authorize]
        [HttpDelete("Api/Nutrition/DeleteIngredientOtherAmount")]
        public async Task<IActionResult> DeleteIngredientOtherAmount(int id)
        {
            var result = await _nutritionService.DeleteRecipeIngredientTypeAmountAsync(id);
            if (result.Succeeded)
            {
                return Ok(new ServiceResult(true));
            }

            return Ok(new ServiceResult(false, result.Errors));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetIngredientSubstitutes")]
        public async Task<IActionResult> GetIngredientSubstitutes(int id)
        {
            return Ok(await _nutritionService.GetIngredientSubstitutesAsync(id));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/AddNewIngredientSubstitute")]
        public async Task<IActionResult> AddNewIngredientSubstitute(RecipeIngredientTypeSubstituteDto model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var createIngredientType = await _nutritionService.CreateRecipeIngredientTypeSubstituteAsync(model);

                if (createIngredientType.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in createIngredientType.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return Ok(new ServiceResult(false, errors));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/EditIngredientSubstitute")]
        public async Task<IActionResult> EditIngredientSubstitute(RecipeIngredientTypeSubstituteDto model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var createIngredientType = await _nutritionService.EditRecipeIngredientTypeSubstituteAsync(model);

                if (createIngredientType.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in createIngredientType.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return Ok(new ServiceResult(false, errors));
        }

        [Log]
        [Authorize]
        [HttpDelete("Api/Nutrition/DeleteIngredientSubstitute")]
        public async Task<IActionResult> DeleteIngredientSubstitute(int id)
        {
            var result = await _nutritionService.DeleteRecipeIngredientTypeSubstituteAsync(id);
            if (result.Succeeded)
            {
                return Ok(new ServiceResult(true));
            }

            return Ok(new ServiceResult(false, result.Errors));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetRecipeDiets")]
        public async Task<IActionResult> GetRecipeDiets(int recipeId)
        {
            return Ok(await _nutritionService.GetDietsByRecipeAsync(recipeId));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetAllDiets")]
        public async Task<IActionResult> GetAllDiets()
        {
            return Ok(await _nutritionService.GetDietsAsync(new DietFilters
            {
                PageSize = 1000,
                Sort = "Order",
                Direction = "ASC"
            }));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/AddDietToRecipe")]
        public async Task<IActionResult> AddDietToRecipe(RecipeDietDto model)
        {
            return Ok(await _nutritionService.AddDietToRecipeAsync(model));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/RemoveDietFromRecipe")]
        public async Task<IActionResult> RemoveDietFromRecipe(RecipeDietDto model)
        {
            return Ok(await _nutritionService.RemoveDietFromRecipeAsync(model));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetRecipeForCustomize")]
        public async Task<IActionResult> GetRecipeForCustomize(string code, int kitchenId)
        {
            return Ok(await _nutritionService.GetRecipeDetailsApi(code, kitchenId));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/CalculateRecipe")]
        public async Task<IActionResult> CalculateRecipe(string code, RecipeChanges changes)
        {
            return Ok(await _nutritionService.CalculateRecipeAsync(code, changes, true));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(UpdateOrder model)
        {
            return Ok(await _nutritionService.ChangeOrderAsync(model));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/SearchSubstitutes")]
        public async Task<IActionResult> SearchSubstitutes(string query, string unit)
        {
            return Ok(await _nutritionService.SearchIngredientForSubstituteAsync(query, unit));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/UpdateRecipeRemarks")]
        public async Task<IActionResult> UpdateRecipeRemarks(RecipeRemarks model)
        {
            return Ok(await _nutritionService.UpdateRecipeRemarksAsync(model));
        }

        [Log]
        [Authorize]
        [HttpDelete("Api/Nutrition/DeleteIngredientTypeSubCategory")]
        public async Task<IActionResult> DeleteIngredientTypeSubCategory(int id)
        {
            var result = await _nutritionService.DeleteIngredientTypeSubCategoryAsync(id);
            if (result.Succeeded)
            {
                return Ok(new ServiceResult(true));
            }

            return Ok(new ServiceResult(false, result.Errors));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetIngredientCategories")]
        public async Task<IActionResult> GetIngredientCategories()
        {
            return Ok(await _nutritionService.GetIngreidnetCategoriesAsync());
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/CreateIngredientTypeSubCategory")]
        public async Task<IActionResult> CreateIngredientTypeSubCategory(IngredientTypeSubCategoryDto model)
        {
            return Ok(await _nutritionService.CreateIngredientTypeSubCategoryAsync(model));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/UpdateRecipeVariables")]
        public async Task<IActionResult> UpdateRecipeVariables(int recipeId)
        {
            return Ok(await _nutritionService.UpdateDefaultRecipeVariablesAsync(recipeId));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetRecipeOwners")]
        public async Task<IActionResult> GetRecipeOwners(int recipeId)
        {
            return Ok(await _nutritionService.GetRecipeOwnersAsync(recipeId));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/AddRecipeOwner")]
        public async Task<IActionResult> AddRecipeOwner(int recipeId, string ownerId)
        {
            return Ok(await _nutritionService.AddRecipeOwnerAsync(recipeId, ownerId));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/RemoveRecipeOwner")]
        public async Task<IActionResult> RemoveRecipeOwner(int recipeId, string ownerId)
        {
            return Ok(await _nutritionService.RemoveRecipeOwnerAsync(recipeId, ownerId));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetRecipeTags")]
        public async Task<IActionResult> GetRecipeTags(int recipeId)
        {
            return Ok(await _nutritionService.GetTagsByRecipeAsync(recipeId));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetAllTags")]
        public async Task<IActionResult> GetAllTags()
        {
            return Ok(await _nutritionService.GetTagsAsync(new TagFilters
            {
                PageSize = 1000,
                Sort = "Order",
                Direction = "ASC"
            }));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/AddTagToRecipe")]
        public async Task<IActionResult> AddTagToRecipe(RecipeTagDto model)
        {
            return Ok(await _nutritionService.AddTagToRecipeAsync(model));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/RemoveTagFromRecipe")]
        public async Task<IActionResult> RemoveTagFromRecipe(RecipeTagDto model)
        {
            return Ok(await _nutritionService.RemoveTagFromRecipeAsync(model));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetPackage")]
        public async Task<IActionResult> GetPackage(int id)
        {
            return Ok(await _nutritionService.GetPackageAsync(id));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/EditPackage")]
        public async Task<IActionResult> EditPackage(EditPackage model)
        {
            return Ok(await _nutritionService.EditPackageAsync(model));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/AddPackageGroup")]
        public async Task<IActionResult> AddPackageGroup(CreateGroup model)
        {
            return Ok(await _nutritionService.AddPackageGroupAsync(model));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/EditPackageGroup")]
        public async Task<IActionResult> EditPackageGroup(EditGroup model)
        {
            return Ok(await _nutritionService.EditPackageGroupAsync(model));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/AddPackageGroupItem")]
        public async Task<IActionResult> AddPackageGroupItem(PackageGroupItemDto model)
        {
            return Ok(await _nutritionService.AddPackageGroupItemAsync(model));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/EditPackageGroupItem")]
        public async Task<IActionResult> EditPackageGroupItem(EditGroupItem model)
        {
            return Ok(await _nutritionService.EditPackageGroupItemAsync(model));
        }

        [Log]
        [Authorize]
        [HttpDelete("Api/Nutrition/DeletePackageGroupItem")]
        public async Task<IActionResult> DeletePackageGroupItem(int id)
        {
            var result = await _nutritionService.DeletePackageGroupItemAsync(id);
            if (result.Succeeded)
            {
                return Ok(new ServiceResult(true));
            }

            return Ok(new ServiceResult(false, result.Errors));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/SearchFoods")]
        public async Task<IActionResult> SearchFoods(string query)
        {
            return Ok(await _nutritionService.GetFoodsAsync(new FoodFilters
            {
                Query = query,
                Sort = "Title",
                Direction = "asc",
                PageSize = 50
            }));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Nutrition/TransferFood")]
        public async Task<IActionResult> TransferFood(TransferFood model)
        {
            return Ok(await _nutritionService.TransferFoodAsync(model));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Nutrition/GetRecipeAllergenAllerts")]
        public async Task<IActionResult> GetRecipeAllergenAllerts(int recipeId)
        {
            return Ok(await _nutritionService.GetRecipeAllergenAllertsAsync(recipeId));
        }
    }
}
