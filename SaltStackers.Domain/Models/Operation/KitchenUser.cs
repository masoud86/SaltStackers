using SaltStackers.Domain.Models.Membership;

namespace SaltStackers.Domain.Models.Operation
{
    public class KitchenUser
    {
        public int Id { get; set; }

        public int KitchenId { get; set; }
        public virtual Kitchen? Kitchen { get; set; }

        public required string UserId { get; set; }
        public virtual AspNetUser? User { get; set; }

        public string? Position { get; set; }

        public bool IsOwner { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
