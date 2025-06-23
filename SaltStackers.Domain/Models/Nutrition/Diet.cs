namespace SaltStackers.Domain.Models.Nutrition;

public class Diet
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Permalink { get; set; }

    public string Icon { get; set; }

    public string Color { get; set; }

    public string? Description { get; set; }

    public string? EmptyDescription { get; set; }

    public int Order { get; set; }

    public bool IsActive { get; set; }

    public bool IsDefault { get; set; }

    public DateTime EditDateTime { get; set; }

    public virtual List<RecipeDiet>? RecipeDiets { get; set; }
}
