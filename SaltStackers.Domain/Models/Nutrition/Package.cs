namespace SaltStackers.Domain.Models.Nutrition;

public class Package
{
    public int Id { get; set; }
    
    public required string Code { get; set; }

    public required string Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public List<PackageAttachment>? Attachments { get; set; }

    public List<PackageGroup>? Groups { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateDateTime { get; set; }
}
