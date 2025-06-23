using SaltStackers.Common.Enums;

namespace SaltStackers.Application.ViewModels.Operation
{
    public class OverheadCostDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal DefaultValue { get; set; }

        public OverheadCategory Category { get; set; }
    }
}
