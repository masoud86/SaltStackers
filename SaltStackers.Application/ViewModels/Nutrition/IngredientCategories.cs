using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class IngredientCategories : Pagination
    {
        public IngredientCategories() : base("EditDateTime")
        {
            Columns = new Dictionary<string, string> {
                {"EditDateTime", "Edit Time"},
                {"Title", Resources.Global.Title}
            };
        }

        public List<IngredientCategoryDto> Items { get; set; }
    }

    public class IngredientCategoryFilters : Pagination
    {
        public IngredientCategoryFilters() : base("EditDateTime")
        {
        }
    }

    public class IngredientCategoryDto : UserLog, IValidatableObject
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Permalink { get; set; }

        public IFormFile? Attachment { get; set; }
        public string? Image { get; set; }

        public int Order { get; set; }

        public DateTime EditDateTime { get; set; }
        public DateTime EditDateTimeLocal => EditDateTime.ConvertFromUtc();

        public List<IngredientSubCategoryDto>? IngredientSubCategories { get; set; }

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

    public class IngredientCategoryApi
    {
        public string Title { get; set; }

        public string Permalink { get; set; }

        public string? Image { get; set; }
        
        public string? ImageUrl { get; set; }

        public int Order { get; set; }
    }
}
