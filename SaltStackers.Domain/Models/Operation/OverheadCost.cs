using SaltStackers.Common.Enums;

namespace SaltStackers.Domain.Models.Operation
{
    public class OverheadCost
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal? DefaultValue { get; set; }

        public virtual OverheadCategory Category { get; set; }
    }
}
