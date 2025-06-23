using System.ComponentModel.DataAnnotations;
using SaltStackers.Common.Helper;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace SaltStackers.Application.ViewModels.Nutrition.Package;

public class PackageDetails
{
    public required string Code { get; set; }

    public required string Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal PayablePrice { get; set; }

    public List<PackageAttachmentApi>? Attachments { get; set; }

    public List<PackageGroupApi>? Groups { get; set; }
}
