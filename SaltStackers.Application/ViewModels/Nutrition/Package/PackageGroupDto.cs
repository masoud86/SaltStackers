namespace SaltStackers.Application.ViewModels.Nutrition.Package;

public class PackageGroupDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public int PackageId { get; set; }

    public PackageDto? Package { get; set; }

    public DateTime CreateDateTime { get; set; }

    public List<PackageGroupItemDto>? Items { get; set; }
}
