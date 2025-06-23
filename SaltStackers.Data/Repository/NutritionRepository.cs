using SaltStackers.Common.Enums;
using SaltStackers.Data.Context;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Membership;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace SaltStackers.Data.Repository
{
    public class NutritionRepository : INutritionRepository
    {
        private readonly AppDbContext _context;

        public NutritionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Package>> GetPackagesAsync(int start, int pageSize, string sortBy, string direction,
            Func<IQueryable<Package>, IIncludableQueryable<Package, object>>? include = null,
            Expression<Func<Package, bool>>? predicate = null)
        {
            IQueryable<Package> query = _context.Packages;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query
                .AsNoTrackingWithIdentityResolution()
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Package>> GetPackagesAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<Package, bool>> predicate = null)
        {
            return await _context.Packages
                .Include(p => p.Attachments)
                .Where(predicate)
                .AsNoTracking()
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetPackagesCountAsync(Expression<Func<Package, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return await _context.Packages.CountAsync();
            }
            return await _context.Packages
                .CountAsync(predicate);
        }

        public async Task<int> CreatePackageAsync(Package model)
        {
            await _context.Packages.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<Package?> GetPackageAsync(int id)
        {
            return await _context.Packages
                .Include(p => p.Groups)
                .ThenInclude(p => p.Items)
                .ThenInclude(p => p.Recipe)
                .ThenInclude(p => p.Food)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Package?> GetPackageAsync(string code)
        {
            return await _context.Packages
                .Include(p => p.Attachments.Where(q => !q.IsMain))
                .Include(p => p.Groups)
                .ThenInclude(p => p.Items)
                .ThenInclude(p => p.Recipe)
                .ThenInclude(p => p.Food)
                .ThenInclude(p => p.Attachments)
                .FirstOrDefaultAsync(p => p.Code.ToUpper() == code.Trim().ToUpper());
        }

        public async Task<bool> EditPackageAsync(Package model)
        {
            _context.Packages.Update(model);
            _context.Entry<Package>(model).State = EntityState.Modified;
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<int> AddPackageGroupAsync(PackageGroup model)
        {
            await _context.PackageGroups.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> EditPackageGroupAsync(PackageGroup model)
        {
            var group = await _context.PackageGroups.FirstOrDefaultAsync(p => p.Id == model.Id);
            if (group != null)
            {
                group.Title = model.Title;
                _context.PackageGroups.Update(group);
                _context.Entry<PackageGroup>(group).State = EntityState.Modified;
                var modified = await _context.SaveChangesAsync();
                return modified > 0;
            }
            return false;
        }

        public async Task<int> AddPackageGroupItemAsync(PackageGroupItem model)
        {
            await _context.PackageGroupItems.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> EditPackageGroupItemAsync(PackageGroupItem model)
        {
            var groupItem = await _context.PackageGroupItems.FirstOrDefaultAsync(p => p.Id == model.Id);
            if (groupItem != null)
            {
                groupItem.Label = model.Label;
                _context.PackageGroupItems.Update(groupItem);
                _context.Entry<PackageGroupItem>(groupItem).State = EntityState.Modified;
                var modified = await _context.SaveChangesAsync();
                return modified > 0;
            }
            return false;
        }

        public async Task<PackageGroupItem?> GetPackageGroupItemAsync(int id)
        {
            return await _context.PackageGroupItems
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> DeletePackageGroupItemAsync(PackageGroupItem model)
        {
            _context.PackageGroupItems.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<int> CreateFoodAsync(Food model)
        {
            await _context.Foods.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> EditFoodAsync(Food model)
        {
            _context.Foods.Update(model);
            _context.Entry<Food>(model).State = EntityState.Modified;
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<bool> DeleteFoodAsync(Food model)
        {
            _context.Foods.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<Food> GetFoodAsync(int? id)
        {
            return await _context.Foods
                .Include(p => p.Attachments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Food> GetFoodByTitleAsync(string title)
        {
            return await _context.Foods.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Title == title);
        }

        public async Task<List<int>> GetFoodIdsByCodesAsync(List<string> codes)
        {
            return await _context.Recipes
                .Where(p => codes.Contains(p.Code.ToLower()))
                .Select(p => p.FoodId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Food>> GetFoodsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<Food, bool>> predicate = null)
        {
            return await _context.Foods
                .Include(p => p.Recipes)
                .Include(p => p.Attachments)
                .Where(predicate)
                .AsNoTrackingWithIdentityResolution()
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetFoodsCountAsync(Expression<Func<Food, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.Foods.CountAsync();
            }
            return await _context.Foods
                .CountAsync(predicate);
        }

        public async Task<int> CreateRecipeAsync(Recipe model)
        {
            var done = false;
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    await _context.Recipes.AddAsync(model);
                    await _context.SaveChangesAsync();

                    await _context.Customizations.AddAsync(new Customization
                    {
                        RecipeId = model.Id,
                        IsDefault = true,
                        Price = model.Price
                    });
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    done = true;
                }
                catch (Exception e)
                {
                    var tt = e;
                    await transaction.RollbackAsync();
                }
            });

            return done ? model.Id : 0;
        }

        public async Task<bool> EditRecipeAsync(Recipe model)
        {
            _context.Recipes.Update(model);
            _context.Entry<Recipe>(model).State = EntityState.Modified;
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            var done = false;
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var recipe = await GetRecipeAsync(id,
                        include: p => p
                                .Include(q => q.RecipeOverheadCosts)
                                .Include(q => q.RecipeIngredientTypeUnits)
                                .ThenInclude(q => q.OtherAmounts)
                                .Include(q => q.RecipeIngredientTypeUnits)
                                .ThenInclude(q => q.Substitutes)
                                .Include(q => q.RecipeTags)
                                .Include(q => q.RecipeDiets));

                    if (recipe != null)
                    {

                        if (recipe.RecipeIngredientTypeUnits != null && recipe.RecipeIngredientTypeUnits.Any())
                        {
                            foreach (var item in recipe.RecipeIngredientTypeUnits)
                            {
                                if (item.OtherAmounts != null && item.OtherAmounts.Any())
                                {
                                    _context.RecipeIngredientTypeAmounts.RemoveRange(item.OtherAmounts);
                                }

                                if (item.Substitutes != null && item.Substitutes.Any())
                                {
                                    _context.RecipeIngredientTypeSubstitutes.RemoveRange(item.Substitutes);
                                }
                            }

                            _context.RecipeIngredientTypeUnits.RemoveRange(recipe.RecipeIngredientTypeUnits);
                            await _context.SaveChangesAsync();
                        }

                        if (recipe.RecipeOverheadCosts != null && recipe.RecipeOverheadCosts.Any())
                        {
                            _context.RecipeOverheadCosts.RemoveRange(recipe.RecipeOverheadCosts);
                            await _context.SaveChangesAsync();
                        }

                        if (recipe.RecipeDiets != null && recipe.RecipeDiets.Any())
                        {
                            _context.RecipeDiets.RemoveRange(recipe.RecipeDiets);
                            await _context.SaveChangesAsync();
                        }

                        if (recipe.RecipeTags != null && recipe.RecipeTags.Any())
                        {
                            _context.RecipeTags.RemoveRange(recipe.RecipeTags);
                            await _context.SaveChangesAsync();
                        }

                        _context.Recipes.Remove(recipe);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        done = true;
                    }
                }
                catch
                {
                    await transaction.RollbackAsync();
                }
            });
            return done;
        }

        public async Task<Recipe?> GetRecipeAsync(int? id,
            Func<IQueryable<Recipe>, IIncludableQueryable<Recipe, object>>? include = null)
        {
            IQueryable<Recipe> query = _context.Recipes;

            if (include != null)
            {
                query = include(query);
            }

            return await query.AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Recipe?> GetRecipeAsync(string code,
            Func<IQueryable<Recipe>, IIncludableQueryable<Recipe, object>>? include = null)
        {
            IQueryable<Recipe> query = _context.Recipes;

            if (include != null)
            {
                query = include(query);
            }

            return await query.AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<List<Recipe>> GetRecipesAsync(int start, int pageSize, string sortBy, string direction,
            Func<IQueryable<Recipe>, IIncludableQueryable<Recipe, object>>? include = null,
            Expression<Func<Recipe, bool>>? predicate = null)
        {
            IQueryable<Recipe> query = _context.Recipes;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query
                .AsNoTrackingWithIdentityResolution()
                .OrderByDescending(p => p.IsNew)
                .ThenByDescending(p => p.Priority.HasValue)
                .ThenBy(p => p.Priority)
                .ThenBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Recipe>> GetRecipeConvertsAsync(int recipeId)
        {
            var recipe = await GetRecipeAsync(recipeId);

            return await _context.Recipes
                .Where(p => p.FoodId == recipe.FoodId && !p.IsOption && p.Id != recipe.Id && p.RecipeType == RecipeType.MealPrep)
                .ToListAsync();
        }

        public async Task<List<RecipeIngredientTypeAmount>> GetIngredientOtherAmountsAsync(int recipeIngredientTypeId)
        {
            return await _context.RecipeIngredientTypeAmounts
                .Where(p => p.RecipeIngredientTypeUnitId == recipeIngredientTypeId)
                .ToListAsync();
        }

        public async Task<RecipeIngredientTypeAmount> GetRecipeIngredientTypeAmountAsync(int id)
        {
            return await _context.RecipeIngredientTypeAmounts
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<RecipeIngredientTypeAmount> GetRecipeIngredientTypeAmountAsync(int recipeIngredientTypeUnitId, decimal amount)
        {
            return await _context.RecipeIngredientTypeAmounts
                .FirstOrDefaultAsync(p => p.RecipeIngredientTypeUnitId == recipeIngredientTypeUnitId && p.Amount == amount);
        }

        public async Task<int> CreateRecipeIngredientTypeAmountAsync(RecipeIngredientTypeAmount model)
        {
            await _context.RecipeIngredientTypeAmounts.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> EditRecipeIngredientTypeAmountAsync(RecipeIngredientTypeAmount model)
        {
            _context.RecipeIngredientTypeAmounts.Update(model);
            _context.Entry<RecipeIngredientTypeAmount>(model).State = EntityState.Modified;
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<bool> DeleteRecipeIngredientTypeAmountAsync(RecipeIngredientTypeAmount model)
        {
            _context.RecipeIngredientTypeAmounts.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<List<RecipeIngredientTypeSubstitute>> GetIngredientSubstitutesAsync(int recipeIngredientTypeId)
        {
            return await _context.RecipeIngredientTypeSubstitutes
                .Include(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .ThenInclude(p => p.Ingredient)
                .Where(p => p.RecipeIngredientTypeUnitId == recipeIngredientTypeId)
                .ToListAsync();
        }

        public async Task<RecipeIngredientTypeSubstitute> GetRecipeIngredientTypeSubstituteAsync(int id)
        {
            return await _context.RecipeIngredientTypeSubstitutes
                .Include(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .ThenInclude(p => p.Ingredient)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<RecipeIngredientTypeSubstitute> GetRecipeIngredientTypeSubstituteAsync(int recipeIngredientTypeId, int ingredientTypeId)
        {
            return await _context.RecipeIngredientTypeSubstitutes
                .Include(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.Unit)
                .Include(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .ThenInclude(p => p.Ingredient)
                .ThenInclude(p => p.Unit)
                .Include(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .ThenInclude(p => p.AllergenAlerts)
                .FirstOrDefaultAsync(p => p.RecipeIngredientTypeUnitId == recipeIngredientTypeId && p.IngredientTypeUnitId == ingredientTypeId);
        }

        public async Task<int> CreateRecipeIngredientTypeSubstituteAsync(RecipeIngredientTypeSubstitute model)
        {
            await _context.RecipeIngredientTypeSubstitutes.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> EditRecipeIngredientTypeSubstituteAsync(RecipeIngredientTypeSubstitute model)
        {
            _context.RecipeIngredientTypeSubstitutes.Update(model);
            _context.Entry<RecipeIngredientTypeSubstitute>(model).State = EntityState.Modified;
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<bool> DeleteRecipeIngredientTypeSubstituteAsync(RecipeIngredientTypeSubstitute model)
        {
            _context.RecipeIngredientTypeSubstitutes.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<List<int>> GetRecipeIdsOwner(string ownerId)
        {
            return await _context.Recipes
                .Include(p => p.RecipeOwners)
                .Where(p => p.RecipeOwners.Any(q => q.UserId == ownerId))
                .Select(p => p.Id).ToListAsync();
        }

        public async Task<List<int>> GetRecipeIdsByDiet(string dietPermalink)
        {
            return await _context.RecipeDiets
                .Include(p => p.Diet)
                .Include(p => p.Recipe)
                .Where(p => p.Diet.Permalink == dietPermalink && p.Recipe.RecipeType == RecipeType.MealPrep)
                .Select(p => p.RecipeId).ToListAsync();
        }

        public async Task<List<int>> GetRecipeIdsByTags(string[] tags)
        {
            return await _context.RecipeTags
                .Include(p => p.Tag)
                .Include(p => p.Recipe)
                .Where(p => p.Recipe.RecipeType == RecipeType.MealPrep && tags.Contains(p.Tag.Permalink))
                .Select(p => p.RecipeId).ToListAsync();
        }

        public async Task<List<Recipe>> GetRecipesByFoodIdAsync(int foodId)
        {
            return await _context.Recipes
                .Include(p => p.RecipeOwners)
                .Where(p => p.FoodId == foodId)
                .ToListAsync();
        }

        public async Task<List<Recipe>> GetFoodRecipesAsync(int foodId)
        {
            return await _context.Recipes
                .Where(p => p.MainMenu && p.FoodId == foodId)
                .ToListAsync();
        }

        public async Task<int> GetRecipesCountAsync(Expression<Func<Recipe, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.Recipes.CountAsync();
            }
            return await _context.Recipes
                .CountAsync(predicate);
        }

        #region Ingredient
        public async Task<int> CreateIngredientAsync(Ingredient model)
        {
            await _context.Ingredients.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> EditIngredientAsync(Ingredient model)
        {
            _context.Ingredients.Update(model);
            _context.Entry<Ingredient>(model).State = EntityState.Modified;
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<bool> DeleteIngredientAsync(Ingredient model)
        {
            _context.Ingredients.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<Ingredient> GetIngredientAsync(int? id)
        {
            return await _context.Ingredients
                .Include(p => p.Unit)
                .Include(p => p.CookingCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Ingredient> GetIngredientByTitleAsync(string title)
        {
            return await _context.Ingredients.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Title == title);
        }

        public async Task<List<Ingredient>> GetIngredientsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<Ingredient, bool>> predicate = null)
        {
            return await _context.Ingredients
                .Include(p => p.Unit)
                .Include(p => p.CookingCategory)
                .Where(predicate)
                .AsNoTrackingWithIdentityResolution()
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetIngredientsCountAsync(Expression<Func<Ingredient, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.Ingredients.CountAsync();
            }
            return await _context.Ingredients
                .CountAsync(predicate);
        }

        #endregion Ingredient

        #region IngredientType

        public async Task<int> CreateIngredientTypeAsync(IngredientType model)
        {
            await _context.IngredientTypes.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> EditIngredientTypeAsync(IngredientType model, List<IngredientTypeAllergenAlert>? allergenAlerts = null)
        {
            var done = false;
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var currentAllergens = await _context.IngredientTypeAllergenAlerts
                        .Where(p => p.IngredientTypeId == model.Id).ToListAsync();

                    var mustRemoveAllergens = currentAllergens.Except(allergenAlerts).ToList();
                    var mustAddAllergens = allergenAlerts.Except(currentAllergens).ToList();

                    if (mustRemoveAllergens != null && mustRemoveAllergens.Any())
                    {
                        _context.IngredientTypeAllergenAlerts.RemoveRange(mustRemoveAllergens);
                    }

                    if (mustAddAllergens != null && mustAddAllergens.Any())
                    {
                        await _context.IngredientTypeAllergenAlerts.AddRangeAsync(mustAddAllergens);
                    }
                    await _context.SaveChangesAsync();

                    _context.IngredientTypes.Update(model);
                    _context.Entry<IngredientType>(model).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    done = true;
                }
                catch (Exception e)
                {
                    var tt = e;
                    await transaction.RollbackAsync();
                }
            });

            return done;
        }

        public async Task<bool> DeleteIngredientTypeAsync(IngredientType model)
        {
            _context.IngredientTypes.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<IngredientType> GetIngredientTypeAsync(int? id)
        {
            return await _context.IngredientTypes
                .Include(p => p.Ingredient)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IngredientType> GetIngredientTypeByTitleAsync(string title)
        {
            return await _context.IngredientTypes.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Title == title);
        }

        public async Task<List<IngredientType>> GetIngredientTypesAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<IngredientType, bool>> predicate = null)
        {
            return await _context.IngredientTypes
                .Include(p => p.IngredientTypeUnits)
                .ThenInclude(p => p.Unit)
                .Include(p => p.IngredientTypeUnits)
                .Include(p => p.Ingredient)
                .Include(p => p.IngredientTypeSubCategories)
                .ThenInclude(p => p.IngredientSubCategory)
                .ThenInclude(p => p.IngredientCategory)
                .Include(p => p.AllergenAlerts)
                .Where(predicate)
                .AsNoTrackingWithIdentityResolution()
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetIngredientTypesCountAsync(Expression<Func<IngredientType, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.IngredientTypes.CountAsync();
            }
            return await _context.IngredientTypes
                .CountAsync(predicate);
        }


        #endregion IngredientType

        #region IngredientTypeUnit

        public async Task<IngredientTypeUnit> GetIngredientTypeUnitByTypeAndUnitId(int typeId, int unitId)
        {
            return await _context.IngredientTypeUnits.FirstOrDefaultAsync(p => p.IngredientTypeId == typeId && p.UnitId == unitId);
        }
        public async Task<int> CreateIngredientTypeUnitAsync(IngredientTypeUnit model)
        {
            await _context.IngredientTypeUnits.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> EditIngredientTypeUnitAsync(IngredientTypeUnit model)
        {
            _context.IngredientTypeUnits.Update(model);
            _context.Entry<IngredientTypeUnit>(model).State = EntityState.Modified;
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<bool> DeleteIngredientTypeUnitAsync(IngredientTypeUnit model)
        {
            var done = false;
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    _context.IngredientTypeUnits.Remove(model);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    done = true;
                }
                catch (Exception e)
                {
                    var tt = e.Message;
                    await transaction.RollbackAsync();
                }
            });
            return done;
        }

        public async Task<IngredientTypeUnit> GetIngredientTypeUnitAsync(int? id)
        {
            return await _context.IngredientTypeUnits.Include(p => p.IngredientType)
                  .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<IngredientTypeUnit>> GetIngredientTypeUnitsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<IngredientTypeUnit, bool>> predicate = null)
        {
            return await _context.IngredientTypeUnits
                .Include(p => p.Unit)
                .Include(p => p.IngredientType)
                .ThenInclude(p => p.Ingredient)
                .Where(predicate)
                .AsNoTrackingWithIdentityResolution()
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetIngredientTypeUnitsCountAsync(Expression<Func<IngredientTypeUnit, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.IngredientTypeUnits.CountAsync();
            }
            return await _context.IngredientTypeUnits
                .CountAsync(predicate);
        }

        public async Task<List<Unit>> GetUnitsAsync()
        {
            return await _context.Units.ToListAsync();
        }

        #endregion IngredientTypeUnit

        #region RecipeIngredientTypeUnit
        public async Task<int> CreateRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnit model)
        {
            await _context.RecipeIngredientTypeUnits.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> EditRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnit model)
        {
            _context.RecipeIngredientTypeUnits.Update(model);
            _context.Entry<RecipeIngredientTypeUnit>(model).State = EntityState.Modified;
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<bool> DeleteRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnit model)
        {
            _context.RecipeIngredientTypeUnits.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<RecipeIngredientTypeUnit> GetRecipeIngredientTypeUnitAsync(int? id)
        {
            return await _context.RecipeIngredientTypeUnits
                .Include(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.Unit)
                .Include(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .ThenInclude(p => p.Ingredient)
                .Include(p => p.Substitutes)
                .ThenInclude(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .Include(p => p.Substitutes)
                .ThenInclude(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.Unit)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<RecipeIngredientTypeUnit>> GetIngredientTypeUnitsByRecipeAsync(int recipeId)
        {
            return await _context.RecipeIngredientTypeUnits
                .Include(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .ThenInclude(p => p.Ingredient)
                .Include(p => p.IngredientTypeUnit.Unit)
                .Include(p => p.OtherAmounts)
                .Include(p => p.Substitutes)
                .ThenInclude(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .ThenInclude(p => p.Ingredient)
                .Where(p => p.RecipeId == recipeId)
                .ToListAsync();
        }

        public async Task<List<RecipeIngredientTypeUnit>> GetRecipeIngredientTypeUnitsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<RecipeIngredientTypeUnit, bool>> predicate = null)
        {
            return await _context.RecipeIngredientTypeUnits
                .Include(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .ThenInclude(p => p.Ingredient)
                .Include(p => p.IngredientTypeUnit.Unit)
                .Include(p => p.OtherAmounts)
                .Include(p => p.Substitutes)
                .ThenInclude(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .ThenInclude(p => p.Ingredient)
                .Where(predicate)
                .AsNoTracking()
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetRecipeIngredientTypeUnitsCountAsync(Expression<Func<RecipeIngredientTypeUnit, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.RecipeIngredientTypeUnits.CountAsync();
            }
            return await _context.RecipeIngredientTypeUnits
                .CountAsync(predicate);
        }


        #endregion RecipeIngredientTypeUnit       

        #region RecipeOverheadCost
        public async Task<int> CreateRecipeOverheadCostAsync(RecipeOverheadCost model)
        {
            await _context.RecipeOverheadCosts.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> EditRecipeOverheadCostAsync(RecipeOverheadCost model)
        {
            _context.RecipeOverheadCosts.Update(model);
            _context.Entry<RecipeOverheadCost>(model).State = EntityState.Modified;
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<bool> DeleteRecipeOverheadCostAsync(RecipeOverheadCost model)
        {
            _context.RecipeOverheadCosts.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<RecipeOverheadCost> GetRecipeOverheadCostAsync(int? id)
        {
            return await _context.RecipeOverheadCosts
                .Include(p => p.OverheadCost)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<RecipeOverheadCost>> GetRecipeOverheadCostsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<RecipeOverheadCost, bool>> predicate = null)
        {
            return await _context.RecipeOverheadCosts
                .Include(p => p.OverheadCost)
                .Where(predicate)
                .AsNoTrackingWithIdentityResolution()
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetRecipeOverheadCostsCountAsync(Expression<Func<RecipeOverheadCost, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.RecipeOverheadCosts.CountAsync();
            }
            return await _context.RecipeOverheadCosts
                .CountAsync(predicate);
        }


        #endregion RecipeOverheadCost       

        #region Units

        public async Task<int> GetUnitsCountAsync(Expression<Func<Unit, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.Units.CountAsync();
            }
            return await _context.Units
                .CountAsync(predicate);
        }

        public async Task<List<Unit>> GetUnitsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<Unit, bool>> predicate = null)
        {
            return await _context.Units
                .Where(predicate)
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        #endregion Units

        #region Diets

        public async Task<int> GetDietsCountAsync(Expression<Func<Diet, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.Diets.CountAsync();
            }
            return await _context.Diets
                .CountAsync(predicate);
        }

        public async Task<List<Diet>> GetDietsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<Diet, bool>> predicate = null)
        {
            return await _context.Diets
                .Where(predicate)
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Diet>> GetDietsByRecipeAsync(int recipeId)
        {
            return await _context.RecipeDiets
                .Include(p => p.Diet)
                .Where(p => p.RecipeId == recipeId)
                .Select(p => p.Diet)
                .ToListAsync();
        }

        public async Task<int> AddDietToRecipeAsync(RecipeDiet model)
        {
            await _context.RecipeDiets.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> RemoveDietFromRecipeAsync(RecipeDiet model)
        {
            _context.RecipeDiets.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        #endregion Diets

        #region Combo

        public async Task<List<Combo>> GetAllCombos()
        {
            return await _context.Combos
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<List<Combo>> GetCombosByCodesAsync(List<string> codes)
        {
            return await _context.Combos
                .Where(p => codes.Contains(p.Code))
                .ToListAsync();
        }

        public async Task<decimal> GetCombosPrice(List<string> codes)
        {
            if (codes == null || !codes.Any())
            {
                return 0;
            }

            return await _context.Combos
                .Where(p => codes.Contains(p.Code))
                .SumAsync(p => p.Price);
        }

        #endregion Combo

        #region Tags

        public async Task<int> GetTagsCountAsync(Expression<Func<Tag, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.Tags.CountAsync();
            }
            return await _context.Tags
                .CountAsync(predicate);
        }

        public async Task<List<Tag>> GetTagsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<Tag, bool>> predicate = null)
        {
            return await _context.Tags
                .Where(predicate)
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Tag>> GetTagsByRecipeAsync(int recipeId)
        {
            return await _context.RecipeTags
                .Include(p => p.Tag)
                .Where(p => p.RecipeId == recipeId)
                .Select(p => p.Tag)
                .ToListAsync();
        }

        public async Task<List<AllergenAlert>> GetRecipeAllergenAllertsAsync(int recipeId)
        {
            return await _context.RecipeIngredientTypeUnits
                .Include(p => p.IngredientTypeUnit)
                .ThenInclude(p => p.IngredientType)
                .ThenInclude(p => p.AllergenAlerts)
                .Where(p => p.RecipeId == recipeId)
                .SelectMany(p => p.IngredientTypeUnit.IngredientType.AllergenAlerts)
                .Select(p => p.AllergenAlert)
                .Distinct()
                .ToListAsync();
        }

        public async Task<int> AddTagToRecipeAsync(RecipeTag model)
        {
            await _context.RecipeTags.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> RemoveTagFromRecipeAsync(RecipeTag model)
        {
            if (model.Id == 0)
            {
                var recipeTag = await _context.RecipeTags
                    .FirstOrDefaultAsync(p => p.RecipeId == model.RecipeId && p.TagId == model.TagId);
                if (recipeTag != null)
                {
                    _context.RecipeTags.Remove(recipeTag);
                }
            }
            else
            {
                _context.RecipeTags.Remove(model);
            }
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        #endregion Tags

        public async Task<List<string>> GetAllRecipesCodeAsync()
        {
            return await _context.Recipes
                .Select(p => p.Code)
                .ToListAsync();
        }

        public async Task<List<int>> GetAllCustomizationIdsAsync()
        {
            return await _context.Customizations
                .Where(p => p.IsDefault)
                .Select(p => p.Id)
                .ToListAsync();
        }

        public async Task<Customization?> GetCustomizationAsync(int id)
        {
            return await _context.Customizations
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Customization?> GetDefaultCustomizationAsync(int recipeId)
        {
            return await _context.Customizations
                .FirstOrDefaultAsync(p => p.RecipeId == recipeId && p.IsDefault);
        }

        public async Task<List<Customization>> GetRecipeUserCustomizationsAsync(int recipeId, string userId)
        {
            return await _context.Customizations
                .Where(p => p.RecipeId == recipeId && !p.IsDefault && p.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> CreateCustomizationAsync(Customization model)
        {
            await _context.Customizations.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public bool UpdateRecipePrice(int id, decimal price)
        {
            var recipe = _context.Recipes.AsNoTracking().FirstOrDefault(p => p.Id == id);
            if (recipe != null)
            {
                recipe.Price = price;
                recipe.CalculateDateTime = DateTime.UtcNow;
                _context.Recipes.Update(recipe);
                _context.Entry(recipe).State = EntityState.Modified;
                var modified = _context.SaveChanges();
                return modified > 0;
            }
            return false;
        }

        public async Task<bool> UpdateRecipeIngredientOrderAsync(int id, int order)
        {
            var recipeIngredient = await _context.RecipeIngredientTypeUnits
                .FirstOrDefaultAsync(p => p.Id == id);

            if (recipeIngredient != null)
            {
                recipeIngredient.Order = order;

                _context.RecipeIngredientTypeUnits.Update(recipeIngredient);
                _context.Entry<RecipeIngredientTypeUnit>(recipeIngredient).State = EntityState.Modified;
                var modified = await _context.SaveChangesAsync();
                return modified > 0;
            }

            return false;
        }

        public async Task<List<IngredientTypeUnit>> SearchIngredientsByUnitAsync(string query, string unit)
        {
            return await _context.IngredientTypeUnits
                .Include(p => p.IngredientType)
                .Include(p => p.Unit)
                .Where(p => p.Unit.Sign == unit && p.IngredientType.DisplayTitle.Contains(query))
                .ToListAsync();
        }

        public async Task<bool> UpdateRecipeAsync(Recipe model)
        {
            _context.Recipes.Update(model);
            _context.Entry<Recipe>(model).State = EntityState.Modified;
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<bool> UpdateCustomizationAsync(Customization model)
        {
            var customization = await _context.Customizations.FindAsync(model.Id);
            if (customization != null)
            {
                customization.Price = model.Price;
                customization.Energy = model.Energy;
                customization.Protein = model.Protein;
                customization.TotalFat = model.TotalFat;
                customization.Carbohydrate = model.Carbohydrate;
                customization.Cholesterol = model.Cholesterol;
                customization.DietaryFiber = model.DietaryFiber;
                customization.Sugars = model.Sugars;
                customization.Sudium = model.Sudium;
                customization.TransFat = model.TransFat;
                customization.SaturatedFat = model.SaturatedFat;
                customization.Iron = model.Iron;
                customization.VitaminA = model.VitaminA;
                customization.VitaminC = model.VitaminC;
                customization.Zinc = model.Zinc;
                customization.CalculateTime = DateTime.UtcNow;

                _context.Customizations.Update(customization);
                var modified = await _context.SaveChangesAsync();
                return modified > 0;
            }

            return false;
        }

        public async Task<IngredientTypeSubCategory> GetIngredientTypeSubCategoryAsync(int id)
        {
            return await _context.IngredientTypeSubCategories
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IngredientTypeSubCategory> GetIngredientTypeSubCategoryAsync(int ingredientTypeId, int ingredientSubCategoryId)
        {
            return await _context.IngredientTypeSubCategories
                .FirstOrDefaultAsync(p => p.IngredientTypeId == ingredientTypeId &&
                p.IngredientSubCategoryId == ingredientSubCategoryId);
        }

        public async Task<bool> DeleteIngredientTypeSubCategoryAsync(IngredientTypeSubCategory model)
        {
            _context.IngredientTypeSubCategories.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<List<IngredientCategory>> GetIngreidnetCategoriesAsync()
        {
            return await _context.IngredientCategories
                .Include(p => p.IngredientSubCategories)
                .ToListAsync();
        }

        public async Task<List<IngredientSubCategory>> GetIngreidnetSubCategoriesAsync(string permalink)
        {
            return await _context.IngredientSubCategories
                .Include(p => p.IngredientCategory)
                .Where(p => p.IngredientCategory.Permalink == permalink.Trim().ToLower())
                .ToListAsync();
        }

        public async Task<int> CreateIngredientTypeSubCategoryAsync(IngredientTypeSubCategory model)
        {
            await _context.IngredientTypeSubCategories.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<List<IngredientTypeUnit>> GetPlateIngredientsAsync(string subCategory)
        {
            return await _context.IngredientTypeUnits
                .Include(p => p.Unit)
                .Include(p => p.IngredientType)
                .ThenInclude(p => p.IngredientTypeSubCategories)
                .ThenInclude(p => p.IngredientSubCategory)
                .Where(p => p.MakeYourOwn)
                .ToListAsync();
        }

        public async Task<List<AspNetUser>> GetRecipeOwnersAsync(int recipeId)
        {
            return await _context.RecipeOwners
                .Include(p => p.User)
                .Where(p => p.RecipeId == recipeId)
                .Select(p => p.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RecipeOwner> GetRecipeOwnerAsync(int id)
        {
            return await _context.RecipeOwners
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<RecipeOwner> GetRecipeOwnerAsync(int recipeId, string ownerId)
        {
            return await _context.RecipeOwners
                .FirstOrDefaultAsync(p => p.RecipeId == recipeId && p.UserId == ownerId);
        }

        public async Task<int> AddRecipeOwnerAsync(RecipeOwner model)
        {
            await _context.RecipeOwners.AddAsync(model);
            var inserted = await _context.SaveChangesAsync();
            return inserted > 0 ? model.Id : 0;
        }

        public async Task<bool> RemoveRecipeOwnerAsync(RecipeOwner model)
        {
            _context.RecipeOwners.Remove(model);
            var modified = await _context.SaveChangesAsync();
            return modified > 0;
        }

        public async Task<List<IngredientCookingCategory>> GetCookingCategoriesAsync()
        {
            return await _context.IngredientCookingCategories
                .OrderBy(p => p.Order).ToListAsync();
        }

        public async Task<IngredientCookingCategory?> GetCookingCategoryAsync(int id)
        {
            return await _context.IngredientCookingCategories
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
