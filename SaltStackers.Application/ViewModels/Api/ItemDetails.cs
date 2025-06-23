using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Nutrition;
using Humanizer;

namespace SaltStackers.Application.ViewModels.Api
{
    public class ItemDetails
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string By { get; set; }

        public decimal DefaultPrice { get; set; }

        public decimal PayablePrice { get; set; }

        public bool Orderable { get; set; }

        public List<string> Ingredients { get; set; }

        public List<FoodAttachmentApi> Attachments { get; set; }

        public List<NutritionFact> NutritionFacts { get; set; }

        public List<OtherSize> Sizes { get; set; }

        public List<Day> PrepDays { get; set; }

        public List<DietApi> Diets { get; set; }

        public List<TagApi> Tags { get; set; }

        public Customize Customize { get; set; }

        public List<ItemDay>? ItemDays { get; set; }
    }

    public class OtherSize
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public bool Selected { get; set; }
    }

    public class Customize
    {
        public List<CustomizeItem> Ingredients { get; set; }

        public List<CustomizeItem> AddOns { get; set; }

        public List<Flag> Flags { get; set; }

        public List<RecipeConvert> Converts { get; set; }

        public List<ComboApi>? Combos { get; set; }
    }

    public class CustomizeItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Order { get; set; }

        public decimal DefaultSize { get; set; }

        public string Unit { get; set; }

        public List<decimal> Sizes { get; set; }

        public List<Substitue> Substitues { get; set; }
    }

    public class Substitue
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsDefault { get; set; }
    }

    public class Flag
    {
        public Flag(string key, string title, bool value)
        {
            Key = key;
            Title = title;
            DefaultValue = value;
        }
        public string Key { get; set; }

        public string Title { get; set; }

        public bool DefaultValue { get; set; }
    }

    public class RecipeConvert
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }
    }

    public class IngredientSort
    {
        public int Order { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }
    }

    public class ItemDay
    {
        public ItemDay()
        {
            DayOfWeek = null;
            Date = null;
        }

        public ItemDay(DayOfWeek dayOfWeek, DateTime date)
        {
            Id = (int)dayOfWeek;
            DayOfWeek = dayOfWeek;
            Date = date;
        }

        public int? Id { get; set; }

        public DayOfWeek? DayOfWeek { get; set; }

        public string Title => DayOfWeek.HasValue && Date.HasValue
            ? $"{Date.Value.ToString("ddd (dd")}th)"
            : "None";

        public DateTime? Date { get; set; }
    }
}
