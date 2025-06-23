namespace SaltStackers.Application.ViewModels.Financial;

public class TaxProfileDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreateDateTime { get; set; }
}
