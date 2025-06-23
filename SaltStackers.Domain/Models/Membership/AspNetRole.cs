using Microsoft.AspNetCore.Identity;

namespace SaltStackers.Domain.Models.Membership
{
    public class AspNetRole : IdentityRole
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string? Icon { get; set; }

        public bool IsLocked { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
