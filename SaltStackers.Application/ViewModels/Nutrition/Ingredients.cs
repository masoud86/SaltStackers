using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Enums;
using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class Ingredients : Pagination
    {
        public Ingredients() : base("EditDateTime")
        {
            Columns = new Dictionary<string, string> {
                {"EditDateTime", "Edit Time"},
                {"CreateDateTime", Resources.Global.CreateTime},
                {"Title", Resources.Global.Title}
            };
        }
        public List<IngredientDto> Items { get; set; }
    }

    public class IngredientFilters : Pagination
    {
        public IngredientFilters() : base("EditDateTime")
        {
        }
    }

    public class IngredientDto : UserLog
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Title", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [StringLength(200, ErrorMessageResourceName = "StringLengthMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string Title { get; set; }

        [Display(Name = "Unit", ResourceType = typeof(Resources.Health))]
        public int? UnitId { get; set; }
        public UnitDto? Unit { get; set; }
        public List<UnitDto>? Units { get; set; }

        [Display(Name = "Order Period")]
        public OrderPeriod OrderPeriod { get; set; }

        public DateTime EditDateTime { get; set; }
        public string EditDateTimeLocal => EditDateTime.ConvertFromUtcString();
    }
}
