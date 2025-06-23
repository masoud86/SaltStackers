using SaltStackers.Domain.Models.Operation;
using System;

namespace SaltStackers.Domain.Models.Nutrition
{
    public class RecipeOverheadCost
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }

        public int OverheadCostId { get; set; }
        public virtual OverheadCost OverheadCost { get; set; }

        public decimal Amount { get; set; }

        public DateTime EditDateTime { get; set; }
    }
}
