using SaltStackers.Common.Enums;
using SaltStackers.Domain.Models.Membership;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace SaltStackers.Domain.Interfaces
{
    public interface INutritionRepository
    {
        #region Package

        Task<List<Package>> GetPackagesAsync(int start, int pageSize, string sortBy, string direction,
            Func<IQueryable<Package>, IIncludableQueryable<Package, object>>? include = null,
            Expression<Func<Package, bool>>? predicate = null);

        Task<List<Package>> GetPackagesAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<Package, bool>> predicate = null);

        Task<int> GetPackagesCountAsync(Expression<Func<Package, bool>>? predicate = null);

        Task<int> CreatePackageAsync(Package model);

        Task<Package?> GetPackageAsync(int id);

        Task<Package?> GetPackageAsync(string code);

        Task<bool> EditPackageAsync(Package model);

        Task<int> AddPackageGroupAsync(PackageGroup model);

        Task<bool> EditPackageGroupAsync(PackageGroup model);

        Task<int> AddPackageGroupItemAsync(PackageGroupItem model);

        Task<bool> EditPackageGroupItemAsync(PackageGroupItem model);

        Task<PackageGroupItem?> GetPackageGroupItemAsync(int id);

        Task<bool> DeletePackageGroupItemAsync(PackageGroupItem model);

        #endregion Package

        #region Foods
        Task<int> CreateFoodAsync(Food model);

        Task<bool> EditFoodAsync(Food model);

        Task<bool> DeleteFoodAsync(Food model);

        Task<Food> GetFoodAsync(int? id);

        Task<Food> GetFoodByTitleAsync(string title);

        Task<List<int>> GetFoodIdsByCodesAsync(List<string> codes);

        Task<List<Food>> GetFoodsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<Food, bool>> predicate = null);

        Task<int> GetFoodsCountAsync(Expression<Func<Food, bool>> predicate = null);

        #endregion Foods

        #region Recipe
        Task<int> CreateRecipeAsync(Recipe model);

        Task<bool> EditRecipeAsync(Recipe model);

        Task<bool> DeleteRecipeAsync(int id);

        Task<Recipe?> GetRecipeAsync(int? id,
            Func<IQueryable<Recipe>, IIncludableQueryable<Recipe, object>>? include = null);

        Task<Recipe?> GetRecipeAsync(string code,
            Func<IQueryable<Recipe>, IIncludableQueryable<Recipe, object>>? include = null);

        Task<List<Recipe>> GetRecipesAsync(int start, int pageSize, string sortBy, string direction,
            Func<IQueryable<Recipe>, IIncludableQueryable<Recipe, object>>? include = null,
            Expression<Func<Recipe, bool>>? predicate = null);

        Task<List<Recipe>> GetRecipesByFoodIdAsync(int foodId);

        Task<List<Recipe>> GetFoodRecipesAsync(int foodId);

        Task<int> GetRecipesCountAsync(Expression<Func<Recipe, bool>> predicate = null);

        Task<List<Recipe>> GetRecipeConvertsAsync(int recipeId);

        Task<List<RecipeIngredientTypeAmount>> GetIngredientOtherAmountsAsync(int recipeIngredientTypeId);

        Task<RecipeIngredientTypeAmount> GetRecipeIngredientTypeAmountAsync(int id);

        Task<RecipeIngredientTypeAmount> GetRecipeIngredientTypeAmountAsync(int recipeIngredientTypeUnitId, decimal amount);

        Task<int> CreateRecipeIngredientTypeAmountAsync(RecipeIngredientTypeAmount model);

        Task<bool> EditRecipeIngredientTypeAmountAsync(RecipeIngredientTypeAmount model);

        Task<bool> DeleteRecipeIngredientTypeAmountAsync(RecipeIngredientTypeAmount model);

        Task<List<RecipeIngredientTypeSubstitute>> GetIngredientSubstitutesAsync(int recipeIngredientTypeId);

        Task<RecipeIngredientTypeSubstitute> GetRecipeIngredientTypeSubstituteAsync(int id);

        Task<RecipeIngredientTypeSubstitute> GetRecipeIngredientTypeSubstituteAsync(int recipeIngredientTypeId, int ingredientTypeId);

        Task<int> CreateRecipeIngredientTypeSubstituteAsync(RecipeIngredientTypeSubstitute model);

        Task<bool> EditRecipeIngredientTypeSubstituteAsync(RecipeIngredientTypeSubstitute model);

        Task<bool> DeleteRecipeIngredientTypeSubstituteAsync(RecipeIngredientTypeSubstitute model);

        Task<List<int>> GetRecipeIdsOwner(string ownerId);

        Task<List<int>> GetRecipeIdsByDiet(string dietPermalink);

        Task<List<int>> GetRecipeIdsByTags(string[] tags);

        #endregion

        #region Ingredient
        Task<int> CreateIngredientAsync(Ingredient model);

        Task<bool> EditIngredientAsync(Ingredient model);

        Task<bool> DeleteIngredientAsync(Ingredient model);

        Task<Ingredient> GetIngredientAsync(int? id);

        Task<Ingredient> GetIngredientByTitleAsync(string title);

        Task<List<Ingredient>> GetIngredientsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<Ingredient, bool>> predicate = null);

        Task<int> GetIngredientsCountAsync(Expression<Func<Ingredient, bool>> predicate = null);

        #endregion Ingredient

        #region IngredientType
        Task<int> CreateIngredientTypeAsync(IngredientType model);

        Task<bool> EditIngredientTypeAsync(IngredientType model, List<IngredientTypeAllergenAlert>? allergenAlerts = null);

        Task<bool> DeleteIngredientTypeAsync(IngredientType model);

        Task<IngredientType> GetIngredientTypeAsync(int? id);

        Task<IngredientType> GetIngredientTypeByTitleAsync(string title);

        Task<List<IngredientType>> GetIngredientTypesAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<IngredientType, bool>> predicate = null);

        Task<int> GetIngredientTypesCountAsync(Expression<Func<IngredientType, bool>> predicate = null);

        #endregion IngredientType

        #region IngredientTypeUnit
        Task<IngredientTypeUnit> GetIngredientTypeUnitByTypeAndUnitId(int typeId, int unitId);
        Task<int> CreateIngredientTypeUnitAsync(IngredientTypeUnit model);

        Task<bool> EditIngredientTypeUnitAsync(IngredientTypeUnit model);

        Task<bool> DeleteIngredientTypeUnitAsync(IngredientTypeUnit model);

        Task<IngredientTypeUnit> GetIngredientTypeUnitAsync(int? id);

        Task<List<IngredientTypeUnit>> GetIngredientTypeUnitsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<IngredientTypeUnit, bool>> predicate = null);

        Task<int> GetIngredientTypeUnitsCountAsync(Expression<Func<IngredientTypeUnit, bool>> predicate = null);

        Task<List<Unit>> GetUnitsAsync();

        #endregion IngredientTypeUnit

        #region RecipeIngredientTypeUnit
        Task<int> CreateRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnit model);

        Task<bool> EditRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnit model);

        Task<bool> DeleteRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnit model);

        Task<RecipeIngredientTypeUnit> GetRecipeIngredientTypeUnitAsync(int? id);

        Task<List<RecipeIngredientTypeUnit>> GetIngredientTypeUnitsByRecipeAsync(int recipeId);

        Task<List<RecipeIngredientTypeUnit>> GetRecipeIngredientTypeUnitsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<RecipeIngredientTypeUnit, bool>> predicate = null);

        Task<int> GetRecipeIngredientTypeUnitsCountAsync(Expression<Func<RecipeIngredientTypeUnit, bool>> predicate = null);

        #endregion

        #region RecipeOverheadCost
        Task<int> CreateRecipeOverheadCostAsync(RecipeOverheadCost model);

        Task<bool> EditRecipeOverheadCostAsync(RecipeOverheadCost model);

        Task<bool> DeleteRecipeOverheadCostAsync(RecipeOverheadCost model);

        Task<RecipeOverheadCost> GetRecipeOverheadCostAsync(int? id);

        Task<List<RecipeOverheadCost>> GetRecipeOverheadCostsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<RecipeOverheadCost, bool>> predicate = null);

        Task<int> GetRecipeOverheadCostsCountAsync(Expression<Func<RecipeOverheadCost, bool>> predicate = null);

        #endregion

        #region Units

        Task<int> GetUnitsCountAsync(Expression<Func<Unit, bool>> predicate = null);

        Task<List<Unit>> GetUnitsAsync(int start, int pageSize, string sortBy, string direction,
           Expression<Func<Unit, bool>> predicate = null);

        #endregion Units

        #region Diets

        Task<int> GetDietsCountAsync(Expression<Func<Diet, bool>> predicate = null);

        Task<List<Diet>> GetDietsAsync(int start, int pageSize, string sortBy, string direction,
           Expression<Func<Diet, bool>> predicate = null);

        Task<List<Diet>> GetDietsByRecipeAsync(int recipeId);

        Task<int> AddDietToRecipeAsync(RecipeDiet model);

        Task<bool> RemoveDietFromRecipeAsync(RecipeDiet model);

        #endregion Diets

        #region Combo

        Task<List<Combo>> GetAllCombos();

        Task<List<Combo>> GetCombosByCodesAsync(List<string> codes);

        Task<decimal> GetCombosPrice(List<string> codes);

        #endregion Combo

        #region Tags

        Task<int> GetTagsCountAsync(Expression<Func<Tag, bool>> predicate = null);

        Task<List<Tag>> GetTagsAsync(int start, int pageSize, string sortBy, string direction,
           Expression<Func<Tag, bool>> predicate = null);

        Task<List<Tag>> GetTagsByRecipeAsync(int recipeId);

        Task<List<AllergenAlert>> GetRecipeAllergenAllertsAsync(int recipeId);

        Task<int> AddTagToRecipeAsync(RecipeTag model);

        Task<bool> RemoveTagFromRecipeAsync(RecipeTag model);

        #endregion Tags

        Task<List<string>> GetAllRecipesCodeAsync();

        Task<List<int>> GetAllCustomizationIdsAsync();

        Task<Customization?> GetCustomizationAsync(int id);

        Task<Customization?> GetDefaultCustomizationAsync(int recipeId);

        Task<List<Customization>> GetRecipeUserCustomizationsAsync(int recipeId, string userId);

        Task<int> CreateCustomizationAsync(Customization model);

        bool UpdateRecipePrice(int id, decimal price);

        Task<bool> UpdateRecipeIngredientOrderAsync(int id, int order);

        Task<List<IngredientTypeUnit>> SearchIngredientsByUnitAsync(string query, string unit);

        Task<bool> UpdateRecipeAsync(Recipe model);

        Task<bool> UpdateCustomizationAsync(Customization model);

        Task<IngredientTypeSubCategory> GetIngredientTypeSubCategoryAsync(int id);

        Task<IngredientTypeSubCategory> GetIngredientTypeSubCategoryAsync(int ingredientTypeId, int ingredientSubCategoryId);

        Task<bool> DeleteIngredientTypeSubCategoryAsync(IngredientTypeSubCategory model);

        Task<List<IngredientCategory>> GetIngreidnetCategoriesAsync();

        Task<List<IngredientSubCategory>> GetIngreidnetSubCategoriesAsync(string permalink);

        Task<int> CreateIngredientTypeSubCategoryAsync(IngredientTypeSubCategory model);

        Task<List<IngredientTypeUnit>> GetPlateIngredientsAsync(string subCategory);

        Task<List<AspNetUser>> GetRecipeOwnersAsync(int recipeId);

        Task<RecipeOwner> GetRecipeOwnerAsync(int id);

        Task<RecipeOwner> GetRecipeOwnerAsync(int recipeId, string ownerId);

        Task<int> AddRecipeOwnerAsync(RecipeOwner model);

        Task<bool> RemoveRecipeOwnerAsync(RecipeOwner model);
    }
}
