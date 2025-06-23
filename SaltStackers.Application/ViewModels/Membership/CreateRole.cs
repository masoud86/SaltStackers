using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class CreateRole
    {
        [DataType(DataType.Text)]
        [Display(Name = "Name", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [StringLength(256, ErrorMessageResourceName = "StringLengthMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "DisplayName", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [StringLength(256, ErrorMessageResourceName = "StringLengthMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string DisplayName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description", ResourceType = typeof(Resources.Global))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string Description { get; set; }

        public string OwnerId { get; set; }
    }
}
