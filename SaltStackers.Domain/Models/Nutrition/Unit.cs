using System.Collections.Generic;

namespace SaltStackers.Domain.Models.Nutrition
{
    public class Unit
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Sign { get; set; }

        public double? ConversionFactor { get; set; }

        public bool HasCustomConversionFactor { get; set; }

        public virtual List<IngredientTypeUnit> IngredientTypeUnits { get; set; }
    }
}
