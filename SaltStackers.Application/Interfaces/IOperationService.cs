using SaltStackers.Application.ViewModels.Operation;
using SaltStackers.Application.ViewModels.Operation.Kitchen;

namespace SaltStackers.Application.Interfaces
{
    public interface IOperationService
    {
        #region Overhead Cost
        Task<List<OverheadCostDto>> GetOverheadCostsAsync(OverheadCostFilters filter);

        Task<OverheadCosts> GetOverheadCostsModelAsync(OverheadCostFilters filter);
        #endregion Overhead Cost

        #region Kitchen

        Task<List<KitchenDto>> GetKitchensAsync(KitchenFilters filter);
        
        Task<List<KitchenApi>> GetKitchensApiAsync(KitchenFilters filter);

        Task<Kitchens> GetKitchensModelAsync(KitchenFilters filter);

        Task<KitchenDto?> GetKitchenAsync(int id);

        Task<KitchenApi?> GetKitchenApiAsync(int id);

        Task<int> CountKitchenAsync();

        #endregion Kitchen

        Task<List<KitchenRecipeDto>> GetRecipesByKitchenAsync(int kitchenId);

        Task<bool> AddRecipeToKitchenAsync(int kitchenId, int recipeId);
        
        Task<bool> RemoveRecipeFromKitchenAsync(int kitchenId, int recipeId);
    }
}
