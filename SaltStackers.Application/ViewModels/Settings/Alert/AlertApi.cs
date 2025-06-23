using SaltStackers.Common.Enums;

namespace SaltStackers.Application.ViewModels.Settings.Alert;

public class AlertApi
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Body { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsDismissable { get; set; }

    public AlertType Type { get; set; }
}
