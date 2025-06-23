using SaltStackers.Domain.Models.Membership;

namespace SaltStackers.Domain.Models.Nutrition
{
    public class RecipeOwner
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public virtual Recipe? Recipe { get; set; }

        public string UserId { get; set; }

        public virtual AspNetUser? User { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
