using SaltStackers.Application.ViewModels.Base;

namespace SaltStackers.Application.ViewModels.Operation.Kitchen;

public class KitchenFilters : Pagination
{
    public KitchenFilters() : base("CreateDateTime")
    {
    }

    public bool ShowAll { get; set; }
}
