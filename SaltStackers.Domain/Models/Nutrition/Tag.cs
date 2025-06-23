using SaltStackers.Common.Enums;

namespace SaltStackers.Domain.Models.Nutrition
{
    public class Tag
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Permalink { get; set; }

        public string Icon { get; set; }

        public int Order { get; set; }

        public TagCategory Category { get; set; }

        public DateTime EditDateTime { get; set; }
    }
}
