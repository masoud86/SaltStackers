namespace SaltStackers.Domain.Models.Nutrition;

public class PackageGroup
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public int PackageId { get; set; }

    public virtual Package? Package { get; set; }

    public DateTime CreateDateTime { get; set; }

    public virtual List<PackageGroupItem>? Items { get; set; }
}
