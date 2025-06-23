using SaltStackers.Application.Filters;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using Microsoft.EntityFrameworkCore;

namespace SaltStackers.Application.Services;

public partial class OperationService : IOperationService
{
    public async Task<List<KitchenDto>> GetKitchensAsync(KitchenFilters filter)
    {
        var kitchens = await _operationRepository
            .GetKitchensAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction,
                include: p =>
                    p.Include(q => q.Zone),
                predicate: OperationFilter.ToExpression(filter));
        return _iMapper.Map<List<KitchenDto>>(kitchens);
    }

    public async Task<List<KitchenApi>> GetKitchensApiAsync(KitchenFilters filter)
    {
        var kitchens = await _operationRepository
            .GetKitchensAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction,
                include: p => p.Include(q => q.Zone),
                predicate: OperationFilter.ToExpression(filter));
        return _iMapper.Map<List<KitchenApi>>(kitchens);
    }

    public async Task<Kitchens> GetKitchensModelAsync(KitchenFilters filter)
    {
        var predicate = OperationFilter.ToExpression(filter);

        var recordTotal = await _operationRepository.GetKitchensCountAsync();

        var recordsFilters = await _operationRepository.GetKitchensCountAsync(predicate);

        return new Kitchens
        {
            Items = await GetKitchensAsync(filter),
            TotalCount = recordTotal,
            FilteredCount = recordsFilters,
            Page = filter.Page
        };
    }

    public async Task<KitchenDto?> GetKitchenAsync(int id)
    {
        var kitchen = await _operationRepository.GetKitchenAsync(id);
        return _iMapper.Map<KitchenDto>(kitchen);
    }

    public async Task<KitchenApi?> GetKitchenApiAsync(int id)
    {
        var kitchen = await _operationRepository.GetKitchenAsync(id);
        return _iMapper.Map<KitchenApi>(kitchen);
    }

    public async Task<int> CountKitchenAsync()
    {
        return await _operationRepository.GetKitchensCountAsync();
    }
}
