using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SaltStackers.Data.Context;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Operation;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace SaltStackers.Data.Repository
{
    public class OperationRepository : IOperationRepository
    {
        private readonly AppDbContext _context;

        public OperationRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Kitchen
        public async Task<List<Kitchen>> GetKitchensAsync(int start, int pageSize, string sortBy, string direction,
            Func<IQueryable<Kitchen>, IIncludableQueryable<Kitchen, object>>? include = null,
            Expression<Func<Kitchen, bool>>? predicate = null)
        {
            IQueryable<Kitchen> query = _context.Kitchens;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();
        }

        public async Task<int> GetKitchensCountAsync(Expression<Func<Kitchen, bool>>? predicate = null)
        {
            IQueryable<Kitchen> query = _context.Kitchens;


            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.CountAsync();
        }

        public async Task<Kitchen?> GetKitchenAsync(int id, Func<IQueryable<Kitchen>, IIncludableQueryable<Kitchen, object>>? include = null)
        {
            IQueryable<Kitchen> query = _context.Kitchens;

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

        #endregion Kitchen

        #region Overhead Costs
        public async Task<int> GetOverheadCostsCountAsync(Expression<Func<OverheadCost, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.OverheadCosts.CountAsync();
            }
            return await _context.OverheadCosts
                .CountAsync(predicate);
        }

        public async Task<List<OverheadCost>> GetOverheadCostsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<OverheadCost, bool>> predicate = null)
        {
            return await _context.OverheadCosts
                .Where(predicate)
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion Overhead Costs

        public async Task<List<Kitchen>> GetAllKitchensAsync()
        {
            return await _context.Kitchens.ToListAsync();
        }

        public void UpdateKitchen(Kitchen model)
        {
            var kitchen = _context.Kitchens.Find(model.Id);
            if (kitchen != null)
            {
                kitchen.CreateDateTime = DateTime.UtcNow;
                _context.Kitchens.Update(kitchen);
                _context.Entry(kitchen).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public async Task<Kitchen?> GetKitchenAsync(int id)
        {
            return await _context.Kitchens
                .Include(p => p.Zone)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<KitchenRecipe?>> GetRecipesByKitchenAsync(int kitchenId)
        {
            return await _context.KitchenRecipes
                .Include(p => p.Recipe)
                .ThenInclude(p => p.Food)
                .ThenInclude(p => p.Attachments)
                .Include(p => p.Recipe)
                .ThenInclude(p => p.RecipeOwners)
                .Where(p => p.KitchenId == kitchenId)
                .ToListAsync();
        }

        public async Task<bool> AddRecipeToKitchenAsync(int kitchenId, int recipeId)
        {
            var exist = _context.KitchenRecipes.Any(p => p.KitchenId == kitchenId && p.RecipeId == recipeId);

            if (!exist)
            {
                await _context.KitchenRecipes.AddAsync(new KitchenRecipe
                {
                    KitchenId = kitchenId,
                    RecipeId = recipeId
                });
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> RemoveRecipeFromKitchenAsync(int kitchenId, int recipeId)
        {
            var kitchenRecipe = await _context.KitchenRecipes.FirstOrDefaultAsync(p => p.KitchenId == kitchenId && p.RecipeId == recipeId);

            if (kitchenRecipe != null)
            {
                _context.KitchenRecipes.Remove(kitchenRecipe);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}
