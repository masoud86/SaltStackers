using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Enums;
using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class IngredientTypes : Pagination
    {
        public IngredientTypes() : base("EditDateTime")
        {
            Columns = new Dictionary<string, string> {
                {"EditDateTime", "Edit Time"},
                {"CreateDateTime", Resources.Global.CreateTime},
                {"Title", Resources.Global.Title}
            };
        }
        public int IngredientId { get; set; }

        public IngredientDto Ingredient { get; set; }

        public List<IngredientTypeDto> Items { get; set; }
    }

    public class IngredientTypeFilters : Pagination
    {
        public IngredientTypeFilters() : base("EditDateTime")
        {
        }
        public int IngredientId { get; set; }

    }

    public class IngredientTypeDto : UserLog
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Title", ResourceType = typeof(Resources.Global))]
        [StringLength(200, ErrorMessageResourceName = "StringLengthMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Display Title")]
        [StringLength(200, ErrorMessageResourceName = "StringLengthMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public string DisplayTitle { get; set; }

        [Display(Name = "BasePrice", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        //[RegularExpression(PatternHelper.Dollar, ErrorMessageResourceName = "RegularExpression",
        //    ErrorMessageResourceType = typeof(Resources.Error))]
        public decimal BasePrice { get; set; }

        public string? MixDescription { get; set; }

        public bool Pchef { get; set; }

        public bool NeedsPrep { get; set; }

        public int IngredientId { get; set; }
        public IngredientDto? Ingredient { get; set; }

        public DateTime EditDateTime { get; set; }
        public string EditDateTimeLocal => EditDateTime.ConvertFromUtcString();

        public List<IngredientTypeUnitDto>? IngredientTypeUnits { get; set; }

        public List<IngredientTypeSubCategoryDto>? IngredientTypeSubCategories { get; set; }

        public List<IngredientTypeAllergenAlertDto>? AllergenAlerts { get; set; }

        public List<AllergenAlert>? Allergens { get; set; }
    }
}
