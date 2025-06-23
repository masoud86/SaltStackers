using SaltStackers.Application.ViewModels.Base;

namespace SaltStackers.Application.ViewModels.Nutrition.Package;

public class EditPackage : UserLog
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}
