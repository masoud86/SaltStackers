using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Enums;
using SaltStackers.Common.Helper;
using System;
using System.Collections.Generic;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class Tags : Pagination
    {
        public Tags() : base("Category")
        {
            Columns = new Dictionary<string, string> {
                {"EditDateTime", "Last modified"},
                {"Title", Resources.Global.Title},
                {"Order", "Order"}
            };
        }

        public List<TagDto> Items { get; set; }
    }

    public class TagFilters : Pagination
    {
        public TagFilters() : base("EditDateTime")
        {
        }
    }

    public class TagDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Permalink { get; set; }

        public string Icon { get; set; }
        public string IconUrl => string.IsNullOrEmpty(Icon) ? "" :
            $"/uploads/diet/icon/{Icon}";

        public int Order { get; set; }

        public TagCategory Category { get; set; }

        public string CategoryTitle => Enum.GetName(typeof(TagCategory), Category);

        public DateTime EditDateTime { get; set; }
        public string EditDateTimeLocal => EditDateTime.ConvertFromUtcString();
    }

    public class TagApi
    {
        public string Title { get; set; }

        public string Permalink { get; set; }

        public string Icon { get; set; }
        public string IconUrl => string.IsNullOrEmpty(Icon) ? "" :
            $"https://publictest.saltstackers.com/uploads/diet/icon/{Icon}";
    }
}
