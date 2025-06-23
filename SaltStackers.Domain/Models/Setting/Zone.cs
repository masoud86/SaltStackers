namespace SaltStackers.Domain.Models.Setting
{
    public class Zone
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public bool IsActive { get; set; }

        public DateTime EditDateTime { get; set; }

        public int CityId { get; set; }

        public virtual City? City { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}
