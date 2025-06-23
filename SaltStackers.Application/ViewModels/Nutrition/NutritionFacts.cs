using SaltStackers.Application.ViewModels.Base;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class NutritionFactsDto : UserLog
    {
        public decimal? Energy { get; set; }
        public decimal? Protein { get; set; }
        public decimal? TotalFat { get; set; }
        public decimal? TransFat { get; set; }
        public decimal? SaturatedFat { get; set; }
        public decimal? Cholesterol { get; set; }
        public decimal? Carbohydrate { get; set; }
        public decimal? DietaryFiber { get; set; }
        public decimal? Sugars { get; set; }
        public decimal? Sudium { get; set; }
        public decimal? Iron { get; set; }
        public decimal? VitaminA { get; set; }
        public decimal? VitaminC { get; set; }
        public decimal? Zinc { get; set; }
    }

    public class NutritionFact
    {
        public NutritionFact(string title, string label, decimal? value, string unit, bool popular = false)
        {
            Title = title;
            Label = label;
            Value = value;
            Unit = unit;
            Popular = popular;
        }

        public string Title { get; set; }

        public string Label { get; set; }

        public decimal? Value { get; set; }

        public string Unit { get; set; }

        public bool Popular { get; set; }
    }
}
