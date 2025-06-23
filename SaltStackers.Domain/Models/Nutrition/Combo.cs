namespace SaltStackers.Domain.Models.Nutrition
{
    public class Combo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string? StriptId { get; set; }

        public bool IsActive { get; set; }

        public DateTime EditDateTime { get; set; }
    }
}
