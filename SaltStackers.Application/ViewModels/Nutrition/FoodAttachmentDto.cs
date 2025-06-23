using SaltStackers.Common.Enums;
using SaltStackers.Common.Helper;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class FoodAttachmentDto
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public bool IsMain { get; set; }

        public MediaType MediaType { get; set; }

        public int FoodId { get; set; }

        public DateTime UploadDateTime { get; set; }
        public string UploadDateTimeLocal => UploadDateTime.ConvertFromUtcString();

        public string? Url { get; set; }
    }

    public class FoodAttachmentApi
    {
        public int FoodId { get; set; }

        public string? FileName { get; set; }

        public string? Url { get; set; }

        public bool IsMain { get; set; }
    }
}
