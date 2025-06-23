using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Enums;
using System.Collections.Generic;

namespace SaltStackers.Application.ViewModels.Operation
{
    public class OverheadCosts : Pagination
    {
        public OverheadCosts() : base("Title")
        {
            Columns = new Dictionary<string, string> {
                {"Title", Resources.Global.Title}
            };
        }

        public List<OverheadCostDto> Items { get; set; }
    }

    public class OverheadCostFilters : Pagination
    {
        public OverheadCostFilters() : base("Title")
        {
        }

        public OverheadCategory OverheadCategory { get; set; }
    }
}
