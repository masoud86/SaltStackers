using SaltStackers.Domain.Models.Financial;
using SaltStackers.Domain.Models.Nutrition;

namespace SaltStackers.Domain.Models.Operation;

public class KitchenRecipe
{
    public int Id { get; set; }

    public int KitchenId { get; set; }

    public int RecipeId { get; set; }

    public DateTime CreateDateTime { get; set; }

    public virtual Kitchen? Kitchen { get; set; }

    public virtual Recipe? Recipe { get; set; }
}
