using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class RecipeIngredientTypeUnits : Pagination
    {
        public RecipeIngredientTypeUnits() : base("EditDateTime")
        {
            Columns = new Dictionary<string, string> {
                {"EditDateTime", Resources.Global.LastModifiedAt}
            };
        }

        public int RecipeId { get; set; }

        public RecipeDto Recipe { get; set; }

        public List<RecipeIngredientTypeUnitDto> Items { get; set; }
    }

    public class RecipeIngredientTypeUnitFilters : Pagination
    {
        public RecipeIngredientTypeUnitFilters() : base("EditDateTime")
        {
        }

        public int RecipeId { get; set; }
    }

    public class RecipeIngredientTypeUnitDto : UserLog
    {
        public int Id { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Health))]
        public decimal Amount { get; set; }

        public bool IsAddOn { get; set; }

        public bool IsDressing { get; set; }

        public int Order { get; set; }

        public decimal PartialPrice => IngredientTypeUnit != null && IngredientTypeUnit.IngredientType != null
            ? Amount * CalculatePartialPrice(IngredientTypeUnit.IngredientType.BasePrice, IngredientTypeUnit.PriceOperator, IngredientTypeUnit.IsPercent, IngredientTypeUnit.PriceFactor)
            : 0;

        public string PartialPriceFormatted => PartialPrice.ToString("0.00");

        public string? IngredientTypeUnitFullTitle { get; set; }

        public string AmountShow => IngredientTypeUnit != null && IngredientTypeUnit.Unit != null
            ? $"{Amount:#.##} {IngredientTypeUnit.Unit.Sign}"
            : "";

        public DateTime EditDateTime { get; set; }
        public string EditDateTimeLocal => EditDateTime.ConvertFromUtcString();


        [Display(Name = "IngredientTypeUnit", ResourceType = typeof(Resources.Health))]
        public int IngredientTypeUnitId { get; set; }
        public IngredientTypeUnitDto? IngredientTypeUnit { get; set; }
        public List<IngredientTypeUnitDto>? IngredientTypeUnits { get; set; }

        public int RecipeId { get; set; }
        public RecipeDto? Recipe { get; set; }


        [Display(Name = "Ingredient", ResourceType = typeof(Resources.Health))]
        public int IngredientId { get; set; }
        public IngredientDto? Ingredient { get; set; }
        public List<IngredientDto>? Ingredients { get; set; }


        [Display(Name = "IngredientType", ResourceType = typeof(Resources.Health))]
        public int IngredientTypeId { get; set; }
        public List<IngredientTypeDto>? IngredientTypes { get; set; }

        public List<RecipeIngredientTypeSubstituteDto>? Substitutes { get; set; }
        public List<RecipeIngredientTypeAmountDto>? OtherAmounts { get; set; }

        public decimal CalculatePartialPrice(decimal basePrice, string priceOperator, bool isPercent, double priceFactor)
        {
            decimal partialPrice = 0;
            switch (priceOperator)
            {
                case "+":
                    {
                        if (isPercent)
                        {
                            partialPrice = basePrice + (basePrice * (decimal)priceFactor) / 100;
                        }
                        else
                        {
                            partialPrice = basePrice + (decimal)priceFactor;
                        }
                    }
                    break;
                case "x":
                    {
                        if (isPercent)
                        {
                            partialPrice = basePrice * (basePrice * (decimal)priceFactor) / 100;
                        }
                        else
                        {
                            partialPrice = basePrice * (decimal)priceFactor;
                        }
                    }
                    break;
                case "-":
                    {
                        if (isPercent)
                        {
                            partialPrice = basePrice - (basePrice * (decimal)priceFactor) / 100;
                        }
                        else
                        {
                            partialPrice = basePrice - (decimal)priceFactor;
                        }
                    }
                    break;
                case "/":
                    {
                        if (isPercent)
                        {
                            partialPrice = basePrice / (basePrice * (decimal)priceFactor) / 100;
                        }
                        else
                        {
                            partialPrice = basePrice / (decimal)priceFactor;
                            // partialPrice = partialPrice * item.Amount;
                        }
                    }
                    break;
                default:
                    break;

            }

            return partialPrice;
        }
    }
}
