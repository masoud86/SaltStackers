using SaltStackers.Application.ViewModels.Api;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Application.ViewModels.Nutrition.Package;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using SaltStackers.Common.Enums;
using SaltStackers.Domain.Models.Nutrition;

namespace SaltStackers.Application.Interfaces
{
    public interface INutritionService
    {
        #region Package

        Task<List<PackageDto>> GetPackagesAsync(PackageFilters filter);

        Task<Packages> GetPackagesModelAsync(PackageFilters filter);

        Task<int> CountPackagesAsyc(PackageFilters filter);

        Task<(bool succeeded, int id)> CreatePackageAsync(string title);

        Task<PackageDto?> GetPackageAsync(int? id);

        Task<PackageDetails?> GetPackageAsync(string code);

        Task<ServiceResult> EditPackageAsync(EditPackage model);

        Task<ServiceResult> AddPackageGroupAsync(CreateGroup model);
        
        Task<ServiceResult> EditPackageGroupAsync(EditGroup model);

        Task<ServiceResult> AddPackageGroupItemAsync(PackageGroupItemDto model);

        Task<ServiceResult> EditPackageGroupItemAsync(EditGroupItem model);

        Task<ServiceResult> DeletePackageGroupItemAsync(int id);

        #endregion Package

        #region Food

        Task<Tuple<ServiceResult, int>> CreateFoodAsync(FoodDto model);

        Task<List<FoodDto>> GetFoodsAsync(FoodFilters filter);

        Task<FoodDto> GetFoodAsync(int? id);

        Task<DeleteFood> GetFoodForDeleteAsync(int? id);

        Task<ServiceResult> UpdateFoodAsync(FoodDto model);

        Task<ServiceResult> DeleteFoodAsync(int? id);

        Task<Foods> GetFoodsModelAsync(FoodFilters filter);

        Task<int> CountFoodsAsyc(FoodFilters filter);

        Task<bool> TransferFoodAsync(TransferFood model);

        #endregion Food

        #region Recipe

        Task<MenuItems> GetMenuItemsAsync(MenuItemFilters filter);

        Task<ServiceResult> CreateRecipeAsync(RecipeDto model);

        Task<List<RecipeDto>> GetRecipesAsync(RecipeFilters filter);

        Task<RecipeDto> GetRecipeAsync(int? id);

        Task<DeleteRecipe> GetRecipeForDeleteAsync(int? id);

        Task<ServiceResult> UpdateRecipeAsync(RecipeDto model);

        Task<ServiceResult> CloneRecipeAsync(RecipeDto model);

        Task<ServiceResult> DeleteRecipeAsync(int? id);

        Task<ServiceResult> SetDefaultRecipeAsync(int? id);

        Task<Recipes> GetRecipesModelAsync(RecipeFilters filter);

        Task<RecipeDetails> GetRecipeDetailsAsync(int? id);

        Task<int> CountRecipesAsyc(RecipeFilters filter);

        Task<List<NutritionFact>> GetNutritionFactsAsync(int recipeId);

        Task<List<RecipeIngredientTypeAmountDto>> GetIngredientOtherAmountsAsync(int recipeIngredientTypeId);

        Task<ServiceResult> CreateRecipeIngredientTypeAmountAsync(RecipeIngredientTypeAmountDto model);

        Task<ServiceResult> EditRecipeIngredientTypeAmountAsync(RecipeIngredientTypeAmountDto model);

        Task<ServiceResult> DeleteRecipeIngredientTypeAmountAsync(int id);

        Task<List<RecipeIngredientTypeSubstituteDto>> GetIngredientSubstitutesAsync(int recipeIngredientTypeId);

        Task<ServiceResult> CreateRecipeIngredientTypeSubstituteAsync(RecipeIngredientTypeSubstituteDto model);

        Task<ServiceResult> EditRecipeIngredientTypeSubstituteAsync(RecipeIngredientTypeSubstituteDto model);

        Task<ServiceResult> DeleteRecipeIngredientTypeSubstituteAsync(int id);

        Task<List<OtherSize>> GetFoodSizesAsync(int foodId, int? recipeId = null);

        Task<ItemDetails> GetRecipeDetailsApi(string code, int kitchenId = 1);

        Task<Tuple<Recipe, decimal, string>> CustomizeRecipeAsync(string code, RecipeChanges changes);

        Task<RecipeVariables> CalculateRecipeAsync(string code, RecipeChanges? changes, bool includeNutritionFacts = false);

        Task<List<RecipeHistory>> GetRecipeHistoriesAsync(string code, string userId);

        Task<ServiceResult> ChangeOrderAsync(UpdateOrder model);

        Task<List<IngredientChangeAnalyzed>> AnalyzeIngredientChanges(RecipeChanges? changes);

        Task<ServiceResult> UpdateDefaultRecipeVariablesAsync(int recipeId);

        Task<List<UserDto>> GetRecipeOwnersAsync(int recipeId);

        Task<ServiceResult> AddRecipeOwnerAsync(int recipeId, string ownerId);

        Task<ServiceResult> RemoveRecipeOwnerAsync(int recipeId, string ownerId);
        #endregion Recipe

        #region Ingredient

        Task<ServiceResult> CreateIngredientAsync(IngredientDto model);

        Task<List<IngredientDto>> GetIngredientsAsync(IngredientFilters filter);

        Task<IngredientDto> GetIngredientAsync(int? id);

        Task<DeleteIngredient> GetIngredientForDeleteAsync(int? id);

        Task<ServiceResult> UpdateIngredientAsync(IngredientDto model);

        Task<ServiceResult> DeleteIngredientAsync(int? id);

        Task<Ingredients> GetIngredientsModelAsync(IngredientFilters filter);

        Task<int> CountIngredientsAsyc(IngredientFilters? filter = null);

        #endregion Ingredient

        #region IngredientType

        Task<ServiceResult> CreateIngredientTypeAsync(IngredientTypeDto model);

        Task<List<IngredientTypeDto>> GetIngredientTypesAsync(IngredientTypeFilters filter);

        Task<IngredientTypeDto> GetIngredientTypeAsync(int? id);

        Task<DeleteIngredientType> GetIngredientTypeForDeleteAsync(int? id);

        Task<ServiceResult> UpdateIngredientTypeAsync(IngredientTypeDto model);

        Task<ServiceResult> DeleteIngredientTypeAsync(int? id);

        Task<IngredientTypes> GetIngredientTypesModelAsync(IngredientTypeFilters filter);

        #endregion IngredientType

        #region IngredientTypeUnit

        Task<ServiceResult> CreateIngredientTypeUnitAsync(IngredientTypeUnitDto model);

        Task<List<IngredientTypeUnitDto>> GetIngredientTypeUnitsAsync(IngredientTypeUnitFilters filter);

        Task<IngredientTypeUnitDto> GetIngredientTypeUnitAsync(int? id);

        Task<DeleteIngredientTypeUnit> GetIngredientTypeUnitForDeleteAsync(int? id);

        Task<ServiceResult> UpdateIngredientTypeUnitAsync(IngredientTypeUnitDto model);

        Task<ServiceResult> DeleteIngredientTypeUnitAsync(int? id);

        Task<IngredientTypeUnits> GetIngredientTypeUnitsModelAsync(IngredientTypeUnitFilters filter);

        #endregion IngredientTypeUnit

        #region RecipeIngredientTypeUnit

        Task<ServiceResult> CreateRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnitDto model);

        Task<List<RecipeIngredientTypeUnitDto>> GetRecipeIngredientTypeUnitsAsync(RecipeIngredientTypeUnitFilters filter);

        Task<RecipeIngredientTypeUnitDto> GetRecipeIngredientTypeUnitAsync(int? id);

        Task<DeleteRecipeIngredientTypeUnit> GetRecipeIngredientTypeUnitForDeleteAsync(int? id);

        Task<ServiceResult> UpdateRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnitDto model);

        Task<ServiceResult> DeleteRecipeIngredientTypeUnitAsync(int? id);

        Task<RecipeIngredientTypeUnits> GetRecipeIngredientTypeUnitsModelAsync(RecipeIngredientTypeUnitFilters filter);

        #endregion RecipeIngredientTypeUnit

        #region RecipeOverheadCost

        Task<ServiceResult> CreateRecipeOverheadCostAsync(RecipeOverheadCostDto model);

        Task<List<RecipeOverheadCostDto>> GetRecipeOverheadCostsAsync(RecipeOverheadCostFilters filter);

        Task<RecipeOverheadCostDto> GetRecipeOverheadCostAsync(int? id);

        Task<DeleteRecipeOverheadCost> GetRecipeOverheadCostForDeleteAsync(int? id);

        Task<ServiceResult> UpdateRecipeOverheadCostAsync(RecipeOverheadCostDto model);

        Task<ServiceResult> DeleteRecipeOverheadCostAsync(int? id);

        Task<RecipeOverheadCosts> GetRecipeOverheadCostsModelAsync(RecipeOverheadCostFilters filter);

        #endregion RecipeOverheadCost

        #region  Units
        Task<List<UnitDto>> GetUnitsAsync(UnitFilters filter);
        Task<Units> GetUnitsModelAsync(UnitFilters filter);

        #endregion  Units

        #region Diets
        Task<List<DietApi>> GetDietsApiAsync(DietFilters filter);

        Task<List<DietDto>> GetDietsAsync(DietFilters filter);

        Task<Diets> GetDietsModelAsync(DietFilters filter);

        Task<List<DietDto>> GetDietsByRecipeAsync(int recipeId);

        Task<ServiceResult> AddDietToRecipeAsync(RecipeDietDto model);

        Task<ServiceResult> RemoveDietFromRecipeAsync(RecipeDietDto model);
        #endregion Diets

        #region Tags
        Task<List<TagApi>> GetTagsApiAsync(TagFilters filter);

        Task<List<TagDto>> GetTagsAsync(TagFilters filter);

        Task<Tags> GetTagsModelAsync(TagFilters filter);

        Task<List<TagDto>> GetTagsByRecipeAsync(int recipeId);

        Task<List<string>> GetRecipeAllergenAllertsAsync(int recipeId);

        Task<ServiceResult> AddTagToRecipeAsync(RecipeTagDto model);

        Task<ServiceResult> RemoveTagFromRecipeAsync(RecipeTagDto model);
        #endregion Tags

        Task<List<string>> GetAllRecipesCodeAsync();

        void UpdateRecipesPrice();

        void CalculateRecipes();

        Task<List<SubstituteSearch>> SearchIngredientForSubstituteAsync(string query, string unit);

        Task<ServiceResult> UpdateRecipeRemarksAsync(RecipeRemarks model);

        Task<ServiceResult> DeleteIngredientTypeSubCategoryAsync(int id);

        Task<List<IngredientCategoryDto>> GetIngreidnetCategoriesAsync();
        
        Task<List<IngredientCategoryApi>> GetIngreidnetCategoriesApiAsync();
        
        Task<List<IngredientSubCategoryApi>> GetIngreidnetSubCategoriesApiAsync(string permalink);

        Task<ServiceResult> CreateIngredientTypeSubCategoryAsync(IngredientTypeSubCategoryDto model);

        Task<List<PlateIngredientApi>> GetPlateIngreidnetsApiAsync(string subCategory);

        Task<List<IngredientCookingCategoryDto>> GetCookingCategoriesAsync();
    }
}
