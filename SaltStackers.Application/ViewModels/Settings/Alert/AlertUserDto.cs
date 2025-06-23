using SaltStackers.Application.ViewModels.Membership;

namespace SaltStackers.Application.ViewModels.Settings.Alert;

public class AlertUserDto
{
    public int Id { get; set; }

    public int AlertId { get; set; }

    public AlertDto? Alert { get; set; }

    public required string UserId { get; set; }

    public UserDto? User { get; set; }

    public DateTime CreateDateTime { get; set; }
}
