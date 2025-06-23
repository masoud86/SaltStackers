using SaltStackers.Application.ViewModels.Api;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Application.ViewModels.Nutrition.Package;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using SaltStackers.Domain.Models.Nutrition;
using SaltStackers.Domain.Models.Operation;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace SaltStackers.Application.Filters
{
    public static class NutritionFilter
    {
        public static ExpressionStarter<Kitchen> ToExpression(KitchenFilters filter)
        {
            var predicate = PredicateBuilder.New<Kitchen>(_ => true);

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    p.Title.Contains(filter.Query, StringComparison.CurrentCultureIgnoreCase));
            }

            return predicate;
        }

        public static ExpressionStarter<Package> ToExpression(PackageFilters filter)
        {
            var predicate = PredicateBuilder.New<Package>(_ => true);

            if (filter.OnlyActives)
            {
                predicate.And(p => p.IsActive);
            }

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    EF.Functions.Like(p.Title, $"%{filter.Query}%"));
            }

            return predicate;
        }

        public static ExpressionStarter<Food> ToExpression(FoodFilters filter)
        {
            var predicate = PredicateBuilder.New<Food>(_ => true);

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    EF.Functions.Like(p.Title, $"%{filter.Query}%"));
            }

            return predicate;
        }

        public static ExpressionStarter<Recipe> ToExpression(RecipeFilters filter)
        {
            var predicate = PredicateBuilder.New<Recipe>(_ => true);

            if (filter.OnlyActives)
            {
                predicate.And(p => p.IsActive);
            }

            if (!string.IsNullOrEmpty(filter.User))
            {
                predicate.And(p => p.RecipeOwners != null && p.RecipeOwners.Any(q => q.UserId == filter.User));
            }

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    EF.Functions.Like(p.Title, $"%{filter.Query}%") ||
                    EF.Functions.Like(p.Food.Title, $"%{filter.Query}%"));
            }
            predicate.And(p =>
                  filter.FoodId == 0 || p.FoodId == filter.FoodId);
            return predicate;
        }

        private static ExpressionStarter<Recipe> FilterByCategory(this ExpressionStarter<Recipe> predicate, MenuItemFilters filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Diet))
            {
                predicate.And(p =>
                    p.MainMenu &&
                    (p.RecipeOwners == null || !p.RecipeOwners.Any()));
            }
            else if (!string.IsNullOrWhiteSpace(filter.Diet) && string.Compare(filter.Diet, "personal", true) == 0 && filter.OwnerId != null)
            {
                predicate.And(p =>
                    p.RecipeOwners != null &&
                    p.RecipeOwners.Any(q => q.UserId == filter.OwnerId) &&
                    p.DefaultInCategory);
            }
            else
            {
                predicate.And(p =>
                    p.RecipeDiets != null &&
                    p.RecipeDiets.Any(q => q.Diet.Permalink == filter.Diet && q.Diet.IsActive) &&
                    p.DefaultInCategory &&
                    (p.RecipeOwners == null || !p.RecipeOwners.Any()));
            }
            return predicate;
        }

        private static ExpressionStarter<Recipe> FilterByTags(this ExpressionStarter<Recipe> predicate, MenuItemFilters filter)
        {
            if (filter.Tags != null && filter.Tags.Any())
            {
                predicate = predicate.And(p => p.RecipeTags.Any(rt => rt.Tag.Permalink == filter.Tags[0]));
            }
            return predicate;
        }

        private static ExpressionStarter<Recipe> FilterByQuery(this ExpressionStarter<Recipe> predicate, MenuItemFilters filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Query))
            {
                predicate.And(p =>
                    
                    (!string.IsNullOrWhiteSpace(p.Title) && EF.Functions.Like(p.Title, $"%{filter.Query}%")) ||
                    (p.Food != null && !string.IsNullOrWhiteSpace(p.Food.Title) && EF.Functions.Like(p.Food.Title, $"%{filter.Query}%")) ||
                    p.Code.ToLower().Contains(filter.Query));
            }
            return predicate;
        }

        public static ExpressionStarter<Recipe> ToExpression(MenuItemFilters filter)
        {
            var predicate = PredicateBuilder.New<Recipe>(p => p.IsActive);

            predicate = predicate.FilterByCategory(filter);
            predicate = predicate.FilterByTags(filter);
            predicate = predicate.FilterByQuery(filter);

            return predicate;
        }

        public static ExpressionStarter<Recipe> ToExpression(int id)
        {
            var predicate = PredicateBuilder.New<Recipe>(_ => true);
            predicate.And(p =>
                   p.FoodId == id);
            return predicate;
        }

        public static ExpressionStarter<Ingredient> ToExpression(IngredientFilters? filter)
        {
            var predicate = PredicateBuilder.New<Ingredient>(_ => true);

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Query))
                {
                    predicate.And(p =>
                        EF.Functions.Like(p.Title, $"%{filter.Query}%"));
                }
            }

            return predicate;
        }

        public static ExpressionStarter<IngredientType> ToExpressionIngredientType(IngredientTypeFilters filter)
        {
            var predicate = PredicateBuilder.New<IngredientType>(_ => true);

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    EF.Functions.Like(p.Title, $"%{filter.Query}%"));
            }
            predicate.And(p =>
                   p.IngredientId == filter.IngredientId);
            return predicate;
        }

        public static ExpressionStarter<IngredientType> ToExpressionIngredientType(int id)
        {
            var predicate = PredicateBuilder.New<IngredientType>(_ => true);
            predicate.And(p =>
                   p.IngredientId == id);
            return predicate;
        }

        public static ExpressionStarter<IngredientTypeUnit> ToExpressionIngredientTypeUnit(IngredientTypeUnitFilters filter)
        {
            var predicate = PredicateBuilder.New<IngredientTypeUnit>(_ => true);

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    EF.Functions.Like(p.Unit.Title, $"%{filter.Query}%"));
            }
            predicate.And(p =>
                   p.IngredientTypeId == filter.IngredientTypeId);
            return predicate;
        }

        public static ExpressionStarter<IngredientTypeUnit> ToExpressionIngredientTypeUnit(int id)
        {
            var predicate = PredicateBuilder.New<IngredientTypeUnit>(_ => true);
            predicate.And(p =>
                   p.IngredientTypeId == id);
            return predicate;
        }

        public static ExpressionStarter<RecipeIngredientTypeUnit> ToExpression(RecipeIngredientTypeUnitFilters filter)
        {
            var predicate = PredicateBuilder.New<RecipeIngredientTypeUnit>(_ => true);
            predicate.And(p =>
                  p.RecipeId == filter.RecipeId);
            if (!string.IsNullOrEmpty(filter.Query))
            {
                //predicate.And(p =>
                //    p.Amount.ToLower().Contains(filter.Query));
            }

            return predicate;
        }

        public static ExpressionStarter<RecipeIngredientTypeUnit> ToExpressionRecipeIngredientTypeUnit(int recipeId)
        {
            var predicate = PredicateBuilder.New<RecipeIngredientTypeUnit>(_ => true);
            predicate.And(p =>
                   p.RecipeId == recipeId);
            return predicate;
        }

        public static ExpressionStarter<RecipeOverheadCost> ToExpression(RecipeOverheadCostFilters filter)
        {
            var predicate = PredicateBuilder.New<RecipeOverheadCost>(_ => true);

            if (filter.RecipeId.HasValue)
            {
                predicate.And(p => p.RecipeId == filter.RecipeId.Value);
            }

            if (!string.IsNullOrEmpty(filter.Query))
            {
                //predicate.And(p =>
                //    p.Amount.ToLower().Contains(filter.Query));
            }

            return predicate;
        }

        public static ExpressionStarter<RecipeOverheadCost> ToExpressionRecipeOverheadCost(int recipeId)
        {
            var predicate = PredicateBuilder.New<RecipeOverheadCost>(_ => true);
            predicate.And(p =>
                   p.RecipeId == recipeId);
            return predicate;
        }

        public static ExpressionStarter<Unit> ToExpression(UnitFilters filter)
        {
            var predicate = PredicateBuilder.New<Unit>(_ => true);

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    EF.Functions.Like(p.Title, $"%{filter.Query}%") ||
                    EF.Functions.Like(p.Category, $"%{filter.Query}%") ||
                    EF.Functions.Like(p.Sign, $"%{filter.Query}%") ||
                    EF.Functions.Like(p.ConversionFactor.ToString(), $"%{filter.Query}%"));
            }

            return predicate;
        }

        public static ExpressionStarter<Diet> ToExpression(DietFilters filter)
        {
            var predicate = PredicateBuilder.New<Diet>(_ => true);

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    EF.Functions.Like(p.Title, $"%{filter.Query}%") ||
                    EF.Functions.Like(p.Permalink, $"%{filter.Query}%"));
            }

            return predicate;
        }
    }
}
