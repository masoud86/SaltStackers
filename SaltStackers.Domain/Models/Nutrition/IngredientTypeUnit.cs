namespace SaltStackers.Domain.Models.Nutrition
{
    public class IngredientTypeUnit : NutritionFacts
    {
        public int Id { get; set; }

        public int IngredientTypeId { get; set; }
        public virtual IngredientType? IngredientType { get; set; }

        public int UnitId { get; set; }
        public virtual Unit? Unit { get; set; }

        public double? ConversionFactor { get; set; }

        #region Pricing
        public string PriceOperator { get; set; }
        public double PriceFactor { get; set; }
        public bool IsPercent { get; set; }
        #endregion Pricing

        #region Unit Convertion
        public string AmountOperator { get; set; }
        public double? AmountFactor { get; set; }
        #endregion Unit Convertion

        #region Make Your Own
        public bool MakeYourOwn { get; set; }
        public decimal? ProfitMargin { get; set; }
        public string? Amounts { get; set; }
        #endregion Make Your Own

        public DateTime EditDateTime { get; set; }
    }
}
