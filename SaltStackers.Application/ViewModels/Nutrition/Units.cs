using SaltStackers.Application.ViewModels.Base;
using System.Collections.Generic;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class Units : Pagination
    {
        public Units() : base("Category")
        {
            Columns = new Dictionary<string, string> {
                {"Category", "Category"},
                {"Title", Resources.Global.Title},
                {"Sign", "Sign"},
                {"ConversionFactor", "Conversion Factor"}
            };
        }

        public List<UnitDto> Items { get; set; }
    }

    public class UnitFilters : Pagination
    {
        public UnitFilters() : base("Category")
        {
        }
    }

    public class UnitDto
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Sign { get; set; }

        public double? ConversionFactor { get; set; }

        public bool HasCustomConversionFactor { get; set; }

        public List<IngredientTypeUnitDto>? IngredientTypeUnits { get; set; }
    }
}
