namespace SaltStackers.Domain.Models.Nutrition;

public class PackageGroupItem
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public required string Label { get; set; }

    public DateTime CreateDateTime { get; set; }

    public int GroupId { get; set; }

    public virtual PackageGroup? Group { get; set; }
}
