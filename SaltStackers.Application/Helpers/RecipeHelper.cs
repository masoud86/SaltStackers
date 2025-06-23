using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Common.Enums;
using SaltStackers.Domain.Models.Nutrition;
using Microsoft.Extensions.Configuration;

namespace SaltStackers.Application.Helpers
{
    public static class RecipeHelper
    {
        public static decimal CalculatePartialPrice(decimal basePrice, string priceOperator, bool isPercent, double priceFactor)
        {
            return priceOperator switch
            {
                "+" => isPercent
                    ? basePrice + (basePrice * (decimal)priceFactor) / 100
                    : basePrice + (decimal)priceFactor,
                "x" => isPercent
                    ? basePrice * (basePrice * (decimal)priceFactor) / 100
                    : basePrice * (decimal)priceFactor,
                "-" => isPercent
                    ? basePrice - (basePrice * (decimal)priceFactor) / 100
                    : basePrice - (decimal)priceFactor,
                "/" => isPercent
                    ? basePrice / (basePrice * (decimal)priceFactor) / 100
                    : basePrice / (decimal)priceFactor,
                _ => 0,
            };
        }

        public static decimal CalculatePrice(this Recipe recipe)
        {
            decimal totalIngredients = 0;
            if (recipe.RecipeIngredientTypeUnits != null && recipe.RecipeIngredientTypeUnits.Where(p => !p.IsAddOn).Any())
            {
                foreach (var ingredient in recipe.RecipeIngredientTypeUnits.Where(p => !p.IsAddOn))
                {
                    var priceOperator = ingredient.IngredientTypeUnit.PriceOperator;
                    var isPercent = ingredient.IngredientTypeUnit.IsPercent;
                    var basePrice = ingredient.IngredientTypeUnit.IngredientType.BasePrice;
                    var priceFactor = ingredient.IngredientTypeUnit.PriceFactor;
                    totalIngredients += ingredient.Amount * CalculatePartialPrice(basePrice, priceOperator, isPercent, priceFactor);
                }
            }

            decimal totalRecipeOverhead = 0;
            if (recipe.RecipeOverheadCosts != null && recipe.RecipeOverheadCosts.Any())
            {
                totalRecipeOverhead = recipe.RecipeOverheadCosts.Sum(p => p.Amount);
            }

            var total = totalIngredients + totalRecipeOverhead;
            var profitMargin = (total * recipe.Food.ProfitMargin) / 100;

            return Math.Round(total + profitMargin, 2);
        }

        public static decimal CalculateNutritionFact(this RecipeIngredientTypeUnit ingredient, string fact)
        {
            if (ingredient == null || ingredient.IngredientTypeUnit == null)
            {
                return 0;
            }

            return ingredient.Amount *
                (decimal)ingredient.IngredientTypeUnit.GetType().GetProperty(fact).GetValue(ingredient.IngredientTypeUnit);
        }

        public static decimal? CalculateNutritionFact(List<RecipeIngredientTypeUnit> ingredients, string item)
        {
            var ingredient = ingredients.Where(p => !p.IsAddOn).Sum(p => p.CalculateNutritionFact(item));
            return Math.Round(ingredient, 0);
        }

        public static List<NutritionFact> CalculateNutritionFacts(List<RecipeIngredientTypeUnit> ingredients)
        {
            return new List<NutritionFact>
            {
                new NutritionFact("Cal", "Energy", CalculateNutritionFact(ingredients, "Energy"), "kcal", true),
                new NutritionFact("Pro", "Protein", CalculateNutritionFact(ingredients, "Protein"), "g", true),
                new NutritionFact("Fat", "Total Fat", CalculateNutritionFact(ingredients, "TotalFat"), "g", true),
                new NutritionFact("Carbs", "Carbohydrate", CalculateNutritionFact(ingredients, "Carbohydrate"), "g", true),
                new NutritionFact("Chol", "Cholesterol", CalculateNutritionFact(ingredients, "Cholesterol"), "mg", true),
                new NutritionFact("Fi", "Dietary Fiber", CalculateNutritionFact(ingredients, "DietaryFiber"), "g", true),
                new NutritionFact("Sugars", "Sugars", CalculateNutritionFact(ingredients, "Sugars"), "g", true),
                new NutritionFact("Sod", "Sodium", CalculateNutritionFact(ingredients, "Sudium"), "mg", true),
                new NutritionFact("Trans Fat", "Trans Fat", CalculateNutritionFact(ingredients, "TransFat"), "g"),
                new NutritionFact("Saturated Fat", "Saturated Fat", CalculateNutritionFact(ingredients, "SaturatedFat"), "g"),
                new NutritionFact("Iron", "Iron", CalculateNutritionFact(ingredients, "Iron"), ""),
                new NutritionFact("Vitamin A", "Vitamin A", CalculateNutritionFact(ingredients, "VitaminA"), ""),
                new NutritionFact("Vitamin C", "Vitamin C", CalculateNutritionFact(ingredients, "VitaminC"), ""),
                new NutritionFact("Zinc", "Zinc", CalculateNutritionFact(ingredients, "Zinc"), "")
            };
        }

        public static List<NutritionFact> CalculateNutritionFacts(this Customization customization)
        {
            return new List<NutritionFact>
            {
                new NutritionFact("Cal", "Energy", customization.Energy, "kcal", true),
                new NutritionFact("Pro", "Protein", customization.Protein, "g", true),
                new NutritionFact("Fat", "Total Fat", customization.TotalFat, "g", true),
                new NutritionFact("Carbs", "Carbohydrate", customization.Carbohydrate, "g", true),
                new NutritionFact("Chol", "Cholesterol", customization.Cholesterol, "mg", true),
                new NutritionFact("Fi", "Dietary Fiber", customization.DietaryFiber, "g", true),
                new NutritionFact("Sugars", "Sugars", customization.Sugars, "g", true),
                new NutritionFact("Sod", "Sodium", customization.Sudium, "mg", true),
                new NutritionFact("Trans Fat", "Trans Fat", customization.TransFat, "g"),
                new NutritionFact("Saturated Fat", "Saturated Fat", customization.SaturatedFat, "g"),
                new NutritionFact("Iron", "Iron", customization.Iron, ""),
                new NutritionFact("Vitamin A", "Vitamin A", customization.VitaminA, ""),
                new NutritionFact("Vitamin C", "Vitamin C", customization.VitaminC, ""),
                new NutritionFact("Zinc", "Zinc", customization.Zinc, "")
            };
        }

        public static List<NutritionFact> CalculateNutritionFact(IngredientTypeUnit ingredient)
        {
            return new List<NutritionFact>
            {
                new NutritionFact("Cal", "Energy", ingredient.Energy, "kcal", true),
                new NutritionFact("Pro", "Protein", ingredient.Protein, "g", true),
                new NutritionFact("Fat", "Total Fat", ingredient.TotalFat, "g", true),
                new NutritionFact("Carbs", "Carbohydrate", ingredient.Carbohydrate, "g", true),
                new NutritionFact("Chol", "Cholesterol", ingredient.Cholesterol, "mg", true),
                new NutritionFact("Fi", "Dietary Fiber", ingredient.DietaryFiber, "g", true),
                new NutritionFact("Sugars", "Sugars", ingredient.Sugars, "g", true),
                new NutritionFact("Sod", "Sodium", ingredient.Sudium, "mg", true),
                new NutritionFact("Trans Fat", "Trans Fat", ingredient.TransFat, "g"),
                new NutritionFact("Saturated Fat", "Saturated Fat", ingredient.SaturatedFat, "g"),
                new NutritionFact("Iron", "Iron", ingredient.Iron, ""),
                new NutritionFact("Vitamin A", "Vitamin A", ingredient.VitaminA, ""),
                new NutritionFact("Vitamin C", "Vitamin C", ingredient.VitaminC, ""),
                new NutritionFact("Zinc", "Zinc", ingredient.Zinc, "")
            };
        }

        public static RecipeChangeType GetRecipeChangeType(decimal defaultSize, float size, int? substituteId)
        {
            if (defaultSize != (decimal)size && !substituteId.HasValue)
            {
                return size == 0 ? RecipeChangeType.RemoveIngredient : RecipeChangeType.ChangeAmount;
            }
            
            if (defaultSize == (decimal)size && substituteId.HasValue)
            {
                return RecipeChangeType.Substitute;
            }

            if (defaultSize != (decimal)size && substituteId.HasValue)
            {
                return size == 0 ? RecipeChangeType.RemoveIngredient : RecipeChangeType.SubstituteChangeAmount;
            }

            return RecipeChangeType.NoChange;
        }

        public static string AnalyzeDescription(RecipeChangeType changeType, string title, string? substituteTitle, float size, string? unit)
        {
            switch (changeType)
            {
                case RecipeChangeType.Substitute:
                    return string.IsNullOrEmpty(substituteTitle) ? "" : substituteTitle;
                case RecipeChangeType.ChangeAmount:
                    return $"{title} ({size.ToString("0.00")} {unit})";
                case RecipeChangeType.SubstituteChangeAmount:
                    return $"{substituteTitle} ({size.ToString("0.00")} {unit})";
                case RecipeChangeType.AddIngredient:
                    return $"Add {title} ({size.ToString("0.00")} {unit})";
                case RecipeChangeType.RemoveIngredient:
                    return $"Remove {title}";
                case RecipeChangeType.AddRemark:
                    return $"{title}";
                default:
                    return "";
            }
        }

        public static IngredientChangeAnalyzed AnalyzeChange(this RecipeIngredientTypeUnit recipeIngredient, int changeId, float size, int? substituteId)
        {
            if (recipeIngredient.IngredientTypeUnit == null ||
                recipeIngredient.IngredientTypeUnit.IngredientType == null)
            {
                throw new NullReferenceException();
            }

            var changeType = GetRecipeChangeType(recipeIngredient.Amount, size, substituteId);
            var title = recipeIngredient.IngredientTypeUnit.IngredientType.DisplayTitle;
            var unit = recipeIngredient.IngredientTypeUnit.Unit?.Sign;
            var substituteTitle = substituteId.HasValue
                ? recipeIngredient.Substitutes?
                    .FirstOrDefault(p => p.IngredientTypeUnitId == substituteId.Value)?
                    .IngredientTypeUnit?.IngredientType?.DisplayTitle
                : "";

            var description = AnalyzeDescription(changeType, title, substituteTitle, size, unit);

            return new IngredientChangeAnalyzed
            {
                Id = changeId,
                Title = title,
                SubstituteId = substituteId,
                SubstituteTitle = substituteTitle,
                Size = size,
                DefaultSize = recipeIngredient.Amount,
                Unit = unit,
                ChangeType = changeType,
                Description = description
            };
        }

        public static string MainImageUrl(this Food food)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            var publicUrl = root.GetSection("PublicUrl").Get<string>();

            var fileName = food.Attachments?.FirstOrDefault()?.FileName;
            return string.IsNullOrEmpty(fileName)
                ? $"{publicUrl}/food/default-small.png"
                : $"{publicUrl}/food/{food.Id}/{fileName}";
        }
    }
}
