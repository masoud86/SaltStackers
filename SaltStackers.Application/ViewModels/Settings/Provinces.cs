using System;

namespace SaltStackers.Application.ViewModels.Settings
{
    public class ProvinceDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public DateTime EditDateTime { get; set; }

        public int CountryId { get; set; }

        public CountryDto Country { get; set; }
    }

    public class ProvinceApi
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CountryId { get; set; }
    }
}
