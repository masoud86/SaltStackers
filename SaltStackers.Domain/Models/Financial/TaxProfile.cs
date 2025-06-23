namespace SaltStackers.Domain.Models.Financial;

public class TaxProfile
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreateDateTime { get; set; }
}
