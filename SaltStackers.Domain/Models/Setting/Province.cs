using System;

namespace SaltStackers.Domain.Models.Setting
{
    public class Province
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public DateTime EditDateTime { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}
