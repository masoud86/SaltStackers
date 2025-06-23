using SaltStackers.Application.ViewModels.Membership;

namespace SaltStackers.Application.ViewModels.Operation.Kitchen;

public class KitchenUserDto
{
    public int Id { get; set; }

    public int KitchenId { get; set; }
    public virtual KitchenDto? Kitchen { get; set; }

    public required string UserId { get; set; }
    public virtual UserDto? User { get; set; }

    public string? Position { get; set; }

    public bool IsOwner { get; set; }

    public DateTime CreateDateTime { get; set; }
}
