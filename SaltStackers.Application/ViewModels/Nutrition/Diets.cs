using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Helper;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class Diets : Pagination
    {
        public Diets() : base("Category")
        {
            Columns = new Dictionary<string, string> {
                {"EditDateTime", "Last modified"},
                {"Title", Resources.Global.Title},
                {"Order", "Order"}
            };
        }

        public List<DietDto> Items { get; set; }
    }

    public class DietFilters : Pagination
    {
        public DietFilters() : base("EditDateTime")
        {
        }
    }

    public class DietDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Permalink { get; set; }

        public string Icon { get; set; }
        public string IconUrl => string.IsNullOrEmpty(Icon) ? "" : $"/uploads/diet/{Icon}";

        public string Color { get; set; }

        public string? Description { get; set; }

        public string? EmptyDescription { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public bool IsDefault { get; set; }

        public DateTime EditDateTime { get; set; }
        public string EditDateTimeLocal => EditDateTime.ConvertFromUtcString();
    }

    public class DietApi
    {
        public string? Title { get; set; }

        public string? Permalink { get; set; }

        public string? Icon { get; set; }

        public string? IconUrl { get; set; }

        public string? Color { get; set; }

        public string? Description { get; set; }

        public string? EmptyDescription { get; set; }
    }
}
