using SaltStackers.Common.Enums;
using SaltStackers.Domain.Models.Financial;
using SaltStackers.Domain.Models.Setting;

namespace SaltStackers.Domain.Models.Operation;

public class Kitchen
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? Subtitle { get; set; }

    public PartnerStatus Status { get; set; }

    public int ZoneId { get; set; }
    public virtual Zone? Zone { get; set; }

    public string? Location { get; set; }

    public double? Longitude { get; set; }

    public double? Latitude { get; set; }

    public string? PostalCode { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Logo { get; set; }

    public int? RecipeTaxProfileId { get; set; }

    public DateTime CreateDateTime { get; set; }

    public virtual TaxProfile? RecipeTaxProfile { get; set; }
}
