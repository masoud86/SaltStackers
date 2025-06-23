using System.ComponentModel.DataAnnotations;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Common.Helper;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace SaltStackers.Application.ViewModels.Nutrition.Package;

public class PackageDto : UserLog, IValidatableObject
{
    public int Id { get; set; }
    
    public required string Code { get; set; }

    public required string Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public List<PackageAttachmentDto>? Attachments { get; set; }

    public List<PackageGroupDto>? Groups { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateDateTime { get; set; }

    public string CreateDateTimeLocal => CreateDateTime.ConvertFromUtcString();

    public string CreateDateTimeHumanized => CreateDateTime == DateTime.MinValue
            ? "" : DateTime.UtcNow.Add(-(DateTime.UtcNow - CreateDateTime)).Humanize();

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
            _ = provider.TryGetContentType(attachment.FileName, out var mimeType);
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
