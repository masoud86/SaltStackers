using Humanizer;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Financial;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Common.Enums;
using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class Recipes : RecipeFilters
    {
        public Recipes()
        {
            Columns = new Dictionary<string, string> {
                {"CreateDateTime", Resources.Global.CreateTime},
                {"Title", Resources.Global.Title}
            };
        }
        public int FoodId { get; set; }

        public FoodDto Food { get; set; }

        public List<RecipeDto> Items { get; set; }
    }

    public class RecipeFilters : Pagination
    {
        public RecipeFilters() : base("CreateDateTime")
        {
        }
        
        public int FoodId { get; set; }

        [Display(Name = "Actives")]
        public bool OnlyActives { get; set; } = true;

        public string? User { get; set; }

        public int? KitchenId { get; set; }
    }

    public class RecipeDto : UserLog
    {
        public int Id { get; set; }

        public int FoodId { get; set; }

        public FoodDto? Food { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Title", ResourceType = typeof(Resources.Global))]
        [StringLength(200, ErrorMessageResourceName = "StringLengthMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string? Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description", ResourceType = typeof(Resources.Global))]
        [StringLength(2000, ErrorMessageResourceName = "StringLengthMax",
           ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
           ErrorMessageResourceType = typeof(Resources.Error))]
        public string? Description { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Recipe Details")]
        public string? RecipeDetails { get; set; }

        [Display(Name = "RecipeType", ResourceType = typeof(Resources.Health))]
        public RecipeType RecipeType { get; set; }
        public string? RecipeTypeTitle => Enum.GetName(typeof(RecipeType), RecipeType);


        [Display(Name = "Skill")]
        public SkillLevel Skill { get; set; }
        public string? SkillTitle => Enum.GetName(typeof(SkillLevel), Skill);

        [Display(Name = "Packaging Time (min)")]
        public int PackagingTime { get; set; }

        public bool MainMenu { get; set; }

        public bool DefaultInCategory { get; set; }

        [Display(Name = "Option")]
        public bool IsOption { get; set; }

        public decimal Price { get; set; }

        public decimal Score { get; set; }

        public string? Code { get; set; }

        public bool AllowNoAppleCider { get; set; }

        public bool AllowNoPepper { get; set; }

        public bool AllowNoSalt { get; set; }

        public bool AllowNoSalmonSkin { get; set; }

        public string? StripeId { get; set; }

        [Display(Name = "Heating Instruction")]
        public string? HeatingInstruction { get; set; }

        [Display(Name = "Routine")]
        public bool IsRoutine { get; set; }

        [Display(Name = "Orderable")]
        public bool Orderable { get; set; }

        [Display(Name = "Priority")]
        public int? Priority { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Recipe Size")]
        public RecipeSize RecipeSize { get; set; }

        [Display(Name = "New")]
        public bool IsNew { get; set; }

        [Display(Name = "Two Step Cooking")]
        public bool IsTwoStepCooking { get; set; }

        public string? RecipeSizeTitle => Enum.GetName(typeof(RecipeSize), RecipeSize);

        [Display(Name = "Personal Chef")]
        public string? PersonalChefId { get; set; }

        public UserDto? PersonalChef { get; set; }

        public DateTime CreateDateTime { get; set; }
        public string CreateDateTimeLocal => CreateDateTime.ConvertFromUtcString();

        public string CreateDateTimeHumanized => CreateDateTime == DateTime.MinValue
            ? "" : DateTime.UtcNow.Add(-(DateTime.UtcNow - CreateDateTime)).Humanize();

        public DateTime? CalculateDateTime { get; set; }
        public string CalculateDateTimeLocal => CalculateDateTime.HasValue
            ? CalculateDateTime.Value.ConvertFromUtcString()
            : "";


        public List<RecipeOverheadCostDto>? RecipeOverheadCosts { get; set; }

        public List<RecipeIngredientTypeUnitDto>? RecipeIngredientTypeUnits { get; set; }

        public List<FoodDto>? Foods { get; set; }

        public List<RecipeOwnerDto>? RecipeOwners { get; set; }

        public List<RecipeDietDto>? RecipeDiets { get; set; }

        public List<UserDto>? PersonalChefs { get; set; }

        public TaxProfileDto? TaxProfile { get; set; }
    }
}
