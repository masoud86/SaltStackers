using SaltStackers.Domain.Models.Membership;

namespace SaltStackers.Domain.Models.Setting;

public class AlertUser
{
    public int Id { get; set; }

    public int AlertId { get; set; }

    public virtual Alert? Alert { get; set; }

    public required string UserId { get; set; }

    public virtual AspNetUser? User { get; set; }

    public bool IsSeen { get; set; }

    public DateTime? ViewDateTime { get; set; }

    public DateTime CreateDateTime { get; set; }
}
