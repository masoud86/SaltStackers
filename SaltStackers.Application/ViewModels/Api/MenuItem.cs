using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Nutrition;

namespace SaltStackers.Application.ViewModels.Api
{
    public class MenuItems : PaginationApi
    {
        public List<MenuItem> Items { get; set; }
    }

    public class MenuItemFilters : Pagination
    {
        public MenuItemFilters() : base("Title")
        {
        }

        public string? Diet { get; set; }

        public string[]? Tags { get; set; }
        
        public List<DayOfWeek>? PrepDays { get; set; }

        public string? OwnerId { get; set; }

        public int? KitchenId { get; set; }
    }

    public class MenuItem
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public decimal Price => PayablePrice;

        public decimal PayablePrice { get; set; }

        public decimal Score { get; set; }

        public List<MenuItemAttachment>? Images { get; set; }

        public List<TagApi>? Tags { get; set; }

        public bool IsNew { get; set; }

        public bool IsBulk { get; set; }
    }

    public class MenuItemAttachment
    {
        public int FoodId { get; set; }

        public string? FileName { get; set; }

        public string? Url { get; set; }

        public bool IsMain { get; set; }
    }
}
