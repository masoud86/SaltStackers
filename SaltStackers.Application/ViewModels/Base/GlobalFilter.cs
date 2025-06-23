using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Base
{
    public class GlobalFilter
    {
        public GlobalFilter(string defaultSort)
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
            get => string.IsNullOrEmpty(_query) ? "" : _query;
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
    }
}
