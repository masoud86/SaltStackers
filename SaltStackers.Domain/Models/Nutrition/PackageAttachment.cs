namespace SaltStackers.Domain.Models.Nutrition;

using SaltStackers.Common.Enums;

public class PackageAttachment
{
    public int Id { get; set; }

    public required string FileName { get; set; }

    public bool IsMain { get; set; }

    public MediaType MediaType { get; set; }

    public int PackageId { get; set; }

    public virtual Package? Package { get; set; }

    public DateTime UploadDateTime { get; set; }
}
