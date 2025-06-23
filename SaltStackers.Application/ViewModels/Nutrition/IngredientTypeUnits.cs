using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class IngredientTypeUnits : Pagination
    {
        public IngredientTypeUnits() : base("EditDateTime")
        {
            Columns = new Dictionary<string, string> {
                {"EditDateTime", Resources.Global.LastModifiedAt}
            };
        }
        public int IngredientTypeId { get; set; }

        public IngredientTypeDto IngredientType { get; set; }

        public List<IngredientTypeUnitDto> Items { get; set; }
    }

    public class IngredientTypeUnitFilters : Pagination
    {
        public IngredientTypeUnitFilters() : base("EditDateTime")
        {
        }
        public int IngredientTypeId { get; set; }
    }

    public class IngredientTypeUnitDto : NutritionFactsDto
    {
        public int Id { get; set; }

        public int IngredientTypeId { get; set; }
        public IngredientTypeDto? IngredientType { get; set; }

        [Display(Name = "Unit", ResourceType = typeof(Resources.Health))]
        public int UnitId { get; set; }
        public UnitDto? Unit { get; set; }
        public List<UnitDto>? Units { get; set; }


        [Display(Name = "ConversionFactor", ResourceType = typeof(Resources.Health))]
        public double? ConversionFactor { get; set; }

        #region Pricing
        [Display(Name = "PriceOperator", ResourceType = typeof(Resources.Health))]
        public string PriceOperator { get; set; }

        [Display(Name = "PriceFactor", ResourceType = typeof(Resources.Health))]
        public double PriceFactor { get; set; }

        [Display(Name = "IsPercent", ResourceType = typeof(Resources.Health))]
        public bool IsPercent { get; set; }
        #endregion Pricing

        #region Unit Convertion
        [Display(Name = "Operator")]
        public string AmountOperator { get; set; }

        [Display(Name = "Factor")]
        public double? AmountFactor { get; set; }
        #endregion Unit Convertion

        #region Make Your Own
        public bool MakeYourOwn { get; set; }
        public decimal? ProfitMargin { get; set; }
        public string? Amounts { get; set; }
        #endregion Make Your Own

        public DateTime EditDateTime { get; set; }
        public string EditDateTimeLocal => EditDateTime.ConvertFromUtcString();
    }
}
