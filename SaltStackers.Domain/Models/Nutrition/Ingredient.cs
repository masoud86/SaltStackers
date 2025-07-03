using SaltStackers.Common.Enums;

namespace SaltStackers.Domain.Models.Nutrition
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int? UnitId { get; set; }

        public virtual Unit? Unit { get; set; }

        public OrderPeriod OrderPeriod { get; set; } = OrderPeriod.Manual;

        public DateTime EditDateTime { get; set; }

        public virtual List<IngredientType>? IngredientTypes { get; set; }
    }
}
