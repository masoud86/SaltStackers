using Microsoft.EntityFrameworkCore.Query;
using SaltStackers.Domain.Models.Operation;
using System.Linq.Expressions;

namespace SaltStackers.Domain.Interfaces
{
    public interface IOperationRepository
    {
        #region Kitchen

        Task<List<Kitchen>> GetKitchensAsync(int start, int pageSize, string sortBy, string direction,
            Func<IQueryable<Kitchen>, IIncludableQueryable<Kitchen, object>>? include = null,
            Expression<Func<Kitchen, bool>>? predicate = null);

        Task<int> GetKitchensCountAsync(Expression<Func<Kitchen, bool>>? predicate = null);

        Task<Kitchen?> GetKitchenAsync(int id, Func<IQueryable<Kitchen>, IIncludableQueryable<Kitchen, object>>? include = null);

        #endregion Kitchen

        #region Overhead Cost
        Task<int> GetOverheadCostsCountAsync(Expression<Func<OverheadCost, bool>> predicate = null);

        Task<List<OverheadCost>> GetOverheadCostsAsync(int start, int pageSize, string sortBy, string direction,
            Expression<Func<OverheadCost, bool>> predicate = null);
        #endregion Overhead Cost

        Task<List<Kitchen>>? GetAllKitchensAsync();

        void UpdateKitchen(Kitchen model);

        Task<Kitchen?> GetKitchenAsync(int id);

        Task<List<KitchenRecipe?>> GetRecipesByKitchenAsync(int kitchenId);

        Task<bool> AddRecipeToKitchenAsync(int kitchenId, int recipeId);
        
        Task<bool> RemoveRecipeFromKitchenAsync(int kitchenId, int recipeId);
    }
}
