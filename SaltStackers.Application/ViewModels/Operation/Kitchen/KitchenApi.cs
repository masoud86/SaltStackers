using System.Text.Json.Serialization;

namespace SaltStackers.Application.ViewModels.Operation.Kitchen;

public class KitchenApi
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Status { get; set; }

    public string? StatusColor
    {
        get
        {
            return (Status?.ToLower()) switch
            {
                "active" => "success",
                "inactive" => "danger",
                "coming soon" => "warning",
                _ => "success",
            };
        }
    }

    public string? Logo { get; set; }

    public string Zone { get; set; }

    public string? Location { get; set; }

    public double? Longitude { get; set; }

    public double? Latitude { get; set; }

    public string? PostalCode { get; set; }

    public string? PhoneNumber { get; set; }

    [JsonIgnore]
    public DateTime OrderWeekStartDate { get; set; }

    [JsonIgnore]
    public DateTime NextOrderWeekChangeTime { get; set; }

    public string OrderingWeek => $"We are getting orders for {OrderWeekStartDate:MMM dd} to {OrderWeekStartDate.AddDays(6):MMM dd}";

    public string NextWeekStart => NextOrderWeekChangeTime.ToString("MMM, dd - HH:mm");
}
