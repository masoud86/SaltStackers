using Microsoft.AspNetCore.Identity;
using SaltStackers.Domain.Models.Nutrition;
using SaltStackers.Domain.Models.Operation;

namespace SaltStackers.Domain.Models.Membership
{
    public class AspNetUser : IdentityUser
    {
        public string? Name { get; set; }

        public bool IsBlocked { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime EditDateTime { get; set; }

        public bool IsAdmin { get; set; }

        public string? Referral { get; set; }

        public DateTime? LastLogin { get; set; }

        public int? KitchenId { get; set; }

        public virtual Kitchen? Kitchen { get; set; }

        public virtual List<Recipe>? Recipes { get; set; }
    }
}
