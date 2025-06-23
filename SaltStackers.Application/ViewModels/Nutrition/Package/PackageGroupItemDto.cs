namespace SaltStackers.Application.ViewModels.Nutrition.Package;

public class PackageGroupItemDto
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public RecipeDto? Recipe { get; set; }

    public required string Label { get; set; }

    public DateTime CreateDateTime { get; set; }

    public int GroupId { get; set; }

    public PackageGroupDto? Group { get; set; }
}
