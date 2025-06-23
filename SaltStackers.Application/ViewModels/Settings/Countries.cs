using System;

namespace SaltStackers.Application.ViewModels.Settings
{
    public class CountryDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public DateTime EditDateTime { get; set; }
    }

    public class CountryApi
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
