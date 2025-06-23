namespace SaltStackers.Application.ViewModels.Nutrition.Package;

public class PackageGroupApi
{
    public required string Title { get; set; }

    public List<PackageGroupItemApi>? Items { get; set; }
}
