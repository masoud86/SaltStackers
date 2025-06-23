using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Base
{
    public class Pagination
    {
        public Pagination(string defaultSort)
        {
            _defaultSort = defaultSort;
        }

        private readonly string _defaultSort;

        private string? _query;
        [DataType("Text")]
        [Display(Name = "Search", ResourceType = typeof(Resources.Global))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthMax", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        public string? Query
        {
            get => string.IsNullOrWhiteSpace(_query) ? "" : _query.Trim();
            set => _query = value;
        }

        private string _sort;
        [Display(Name = "SortBy", ResourceType = typeof(Resources.Global))]
        public string Sort
        {
            get => string.IsNullOrEmpty(_sort) ? _defaultSort : _sort;
            set => _sort = value;
        }

        [Display(Name = "Direction", ResourceType = typeof(Resources.Global))]
        public string Direction { get; set; } = "Desc";

        private int? _pageSize;
        [Display(Name = "PageSize", ResourceType = typeof(Resources.Global))]
        public int PageSize
        {
            get => _pageSize ?? 10;
            set => _pageSize = value;
        }

        private int? _page;
        [Display(Name = "Page", ResourceType = typeof(Resources.Global))]
        public int Page
        {
            get => _page ?? 1;
            set => _page = value;
        }

        public int Start => Page == 1 ? 0 : ((Page - 1) * (PageSize));

        public int FilteredCount { get; set; }

        public int TotalCount { get; set; }

        public string PageUrl { get; set; } = "/";

        public Dictionary<string, string> SortDirections =>
            new Dictionary<string, string> {
                {"Desc", Resources.Global.Descending},
                {"Asc", Resources.Global.Ascending}
            };

        public Dictionary<int, string> PageSizes =>
            new Dictionary<int, string> {
                {10, "10"},
                {20, "20"},
                {50, "50"},
                {100, "100"}
            };

        public Dictionary<string, string> Columns { get; set; }
    }

    public class PaginationSimple
    {
        public int FilteredCount { get; set; }

        public int TotalCount { get; set; }

        public string PageUrl { get; set; } = "/";

        public Dictionary<string, string> SortDirections =>
            new Dictionary<string, string> {
                {"Desc", Resources.Global.Descending},
                {"Asc", Resources.Global.Ascending}
            };

        public Dictionary<int, string> PageSizes =>
            new Dictionary<int, string> {
                {10, "10"},
                {20, "20"},
                {50, "50"},
                {100, "100"}
            };

        public Dictionary<string, string>? Columns { get; set; }

        private int? _pageSize;
        [Display(Name = "PageSize", ResourceType = typeof(Resources.Global))]
        public int PageSize
        {
            get => _pageSize ?? 10;
            set => _pageSize = value;
        }

        public int Start => Page == 1 ? 0 : ((Page - 1) * (PageSize));

        private int? _page;
        [Display(Name = "Page", ResourceType = typeof(Resources.Global))]
        public int Page
        {
            get => _page ?? 1;
            set => _page = value;
        }
    }

    public class PaginationApi
    {
        private int? _page;
        [Display(Name = "Page", ResourceType = typeof(Resources.Global))]
        public int Page
        {
            get => _page ?? 1;
            set => _page = value;
        }

        public int FilteredCount { get; set; }

        public int TotalCount { get; set; }
    }
}
