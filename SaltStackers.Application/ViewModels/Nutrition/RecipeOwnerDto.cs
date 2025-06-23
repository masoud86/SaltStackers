using SaltStackers.Application.ViewModels.Membership;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class RecipeOwnerDto
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public RecipeDto? Recipe { get; set; }

        public string UserId { get; set; }

        public UserDto? User { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
