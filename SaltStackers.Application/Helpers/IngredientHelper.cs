using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Domain.Models.Nutrition;

namespace SaltStackers.Application.Helpers;

public static class IngredientHelper
{
    public static decimal CalculatePrice(decimal basePrice, string priceOperator, bool isPercent, double priceFactor)
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

    public static List<double>? ConvertAmounts(string? amounts)
    {
        return string.IsNullOrEmpty(amounts)
                ? null
                : amounts.Trim().Split(',').OrderBy(p => p).Select(double.Parse).ToList();
    }

    public static decimal CalculateMakePrice(decimal basePrice, string priceOperator, bool isPercent, double priceFactor, double? amount, decimal? profitMargin)
    {
        var partialPrice = CalculatePrice(basePrice, priceOperator, isPercent, priceFactor);
        var amountPrice = partialPrice * (decimal)amount.Value;
        profitMargin = profitMargin.HasValue ? profitMargin.Value : 0;
        return amountPrice + profitMargin.Value;
    }

    public static decimal CalculateMakePrice(decimal basePrice, string priceOperator, bool isPercent, double priceFactor, string? amounts, decimal? profitMargin)
    {
        var amount = ConvertAmounts(amounts)?.FirstOrDefault();
        return CalculateMakePrice(basePrice, priceOperator, isPercent, priceFactor, amount, profitMargin);
    }

    public static decimal ToBaseUnit(this decimal amount, Unit unit, double? conversionFactor)
    {
        if (unit != null)
        {
            if (unit.HasCustomConversionFactor && conversionFactor != null)
            {
                return amount * (decimal)conversionFactor;
            }

            if (unit.ConversionFactor != null)
            {
                return amount * (decimal)unit.ConversionFactor;
            }
        }

        return amount;
    }

    public static decimal ToRawUnit(this decimal amount, IngredientTypeUnit ingredientTypeUnit)
    {
        if (amount == 0)
        {
            return 0;
        }

        var itemUnit = ingredientTypeUnit.Unit;
        var rawUnit = ingredientTypeUnit.IngredientType?.Ingredient?.Unit;

        if (itemUnit is null || rawUnit is null)
        {
            return 0;
        }

        var conversionFactor = itemUnit.ConversionFactor.HasValue
        ? itemUnit.ConversionFactor.Value
        : ingredientTypeUnit.ConversionFactor ?? 0;
        var itemBase = (decimal)conversionFactor * amount;
        var itemBaseRaw = ingredientTypeUnit.AmountOperator == "+"
            ? itemBase + (itemBase * (decimal)ingredientTypeUnit.AmountFactor / 100)
            : itemBase - (itemBase * (decimal)ingredientTypeUnit.AmountFactor / 100);

        //if (itemUnit.Id != rawUnit.Id && rawUnit.ConversionFactor.HasValue)
        //{
        //    itemBaseRaw /= (decimal)rawUnit.ConversionFactor.Value;
        //}

        return itemBaseRaw;
    }
}
