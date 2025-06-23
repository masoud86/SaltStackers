using SaltStackers.Common.Enums;

namespace SaltStackers.Domain.Models.Nutrition
{
    public class FoodAttachment
    {
        public int Id { get; set; }

        public required string FileName { get; set; }

        public bool IsMain { get; set; }

        public MediaType MediaType { get; set; }

        public int FoodId { get; set; }

        public DateTime UploadDateTime { get; set; }
    }
}
