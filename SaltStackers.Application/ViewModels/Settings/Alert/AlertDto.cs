using SaltStackers.Common.Enums;
using SaltStackers.Domain.Models.Setting;

namespace SaltStackers.Application.ViewModels.Settings.Alert;

public class AlertDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Body { get; set; }

    public string? Image { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public bool IsPublic { get; set; }

    public bool NeedTracking { get; set; }

    public bool IsDismissable { get; set; }

    public bool IsActive { get; set; }

    public AlertType Type { get; set; }

    public DateTime CreateDateTime { get; set; }

    public List<AlertUser>? AlertUsers { get; set; }
}
