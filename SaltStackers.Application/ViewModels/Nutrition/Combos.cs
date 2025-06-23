using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class Combos : Pagination
    {
        public Combos() : base("EditDateTime")
        {
            Columns = new Dictionary<string, string> {
                {"EditDateTime", "Edit Time"},
                {"Title", Resources.Global.Title}
            };
        }

        public List<FoodDto> Items { get; set; }
    }

    public class ComboFilters : Pagination
    {
        public ComboFilters() : base("EditDateTime")
        {
        }
    }

    public class ComboDto : UserLog, IValidatableObject
    {
        public int Id { get; set; }

        public string Code { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Title", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [StringLength(200, ErrorMessageResourceName = "StringLengthMax",
            ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression",
            ErrorMessageResourceType = typeof(Resources.Error))]
        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string? StriptId { get; set; }

        public IFormFile? Attachment { get; set; }

        public bool IsActive { get; set; }

        public DateTime EditDateTime { get; set; }
        public string EditDateTimeLocal => EditDateTime.ConvertFromUtcString();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Attachment != null)
            {
                var allowedExtensions = new[] { "jpg", "jpeg", "png" };
                var allowdMimeTypes = new[] { "image/jpeg", "image/png" };
                if (Attachment.Length > 2000000)
                {
                    yield return
                        new ValidationResult("The size of file " + Attachment.FileName + "is more than 2 mb.",
                        new List<string> { "Attachments" });
                }

                var fileExtension = Path.GetExtension(Attachment.FileName.ToLower()).Substring(1);
                if (!allowedExtensions.Contains(fileExtension))
                {
                    yield return
                        new ValidationResult("File " + Attachment.FileName + " is not permitted.",
                        new List<string> { "Attachments" });
                }

                var provider = new FileExtensionContentTypeProvider();
                provider.TryGetContentType(Attachment.FileName, out string mimeType);
                if (!allowdMimeTypes.Contains(mimeType))
                {
                    yield return
                        new ValidationResult("File " + Attachment.FileName + " is not permitted.",
                        new List<string> { "Attachments" });
                }
            }
        }
    }

    public class ComboApi
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl => $"https://publictest.saltstackers.com/combo/{Code}.jpg";
    }
}
