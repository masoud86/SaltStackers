namespace SaltStackers.Application.ViewModels.Settings
{
    public class ZoneDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public bool IsActive { get; set; }

        public DateTime EditDateTime { get; set; }

        public int CityId { get; set; }

        public CityDto? City { get; set; }

        public decimal DeliveryPrice { get; set; }
    }

    public class ZoneApi
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public int CityId { get; set; }
    }
}
