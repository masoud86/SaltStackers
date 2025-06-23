using System;

namespace SaltStackers.Application.ViewModels.Settings
{
    public class CityDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public DateTime EditDateTime { get; set; }

        public int ProvinceId { get; set; }

        public ProvinceDto Province { get; set; }
    }

    public class CityApi
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ProvinceId { get; set; }
    }
}
