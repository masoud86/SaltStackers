using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Helper;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class Foods : Pagination
    {
        public Foods() : base("CreateDateTime")
        {
            Columns = new Dictionary<string, string> {
                {"CreateDateTime", Resources.Global.CreateTime},
                {"Title", Resources.Global.Title}
            };
        }

        public List<FoodDto>? Items { get; set; }
    }

    public class FoodFilters : Pagination
    {
        public FoodFilters() : base("CreateDateTime")
        {
        }
    }

    public class FoodDto : UserLog, IValidatableObject
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

        [DataType(DataType.Text)]
        [Display(Name = "Profit Margin (%)")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public int ProfitMargin { get; set; }

        public DateTime CreateDateTime { get; set; }
        public string CreateDateTimeLocal => CreateDateTime.ConvertFromUtcString();
        public string CreateDateTimeHumanized => CreateDateTime == DateTime.MinValue
            ? "" : DateTime.UtcNow.Add(-(DateTime.UtcNow - CreateDateTime)).Humanize();

        public List<RecipeDto>? Recipes { get; set; }

        public List<string>? Images { get; set; }

        public List<FoodAttachmentDto>? Attachments { get; set; }

        public List<IFormFile>? Uploads { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Uploads != null && Uploads.Count > 0)
            {
                var allowedExtensions = new[] { "jpg", "jpeg", "png" };
                var allowdMimeTypes = new[] { "image/jpeg", "image/png" };
                foreach (var attachment in Uploads)
                {
                    if (attachment.Length > 2000000)
                    {
                        yield return
                            new ValidationResult("The size of file " + attachment.FileName + "is more than 2 mb.",
                            new List<string> { "Attachments" });
                    }

                    var fileExtension = Path.GetExtension(attachment.FileName.ToLower()).Substring(1);
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        yield return
                            new ValidationResult("File " + attachment.FileName + " is not permitted.",
                            new List<string> { "Attachments" });
                    }

                    var provider = new FileExtensionContentTypeProvider();
                    provider.TryGetContentType(attachment.FileName, out string mimeType);
                    if (!allowdMimeTypes.Contains(mimeType))
                    {
                        yield return
                            new ValidationResult("File " + attachment.FileName + " is not permitted.",
                            new List<string> { "Attachments" });
                    }
                }
            }
        }
    }
}
