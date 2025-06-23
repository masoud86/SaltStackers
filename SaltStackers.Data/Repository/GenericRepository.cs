using SaltStackers.Data.Context;
using SaltStackers.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace SaltStackers.Data.Repository;

public class GenericRepository<TEntity>(AppDbContext context) : IGenericRepository<TEntity> where TEntity : class
{
    public async Task<TEntity?> FirstAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        Expression<Func<object, TEntity>>? select = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (include is not null)
        {
            query = include(query);
        }

        if (select is not null)
        {
            query = query.Select(select);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<TEntity>> GetAsync(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<object, TEntity>>? select = null,
        int? start = null, int? pageSize = null,
        string? sortBy = null, string? direction = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (select is not null)
        {
            query = query.Select(select);
        }

        if (!string.IsNullOrEmpty(sortBy) || !string.IsNullOrEmpty(direction))
        {
            sortBy = string.IsNullOrEmpty(sortBy) ? "Id" : sortBy;
            direction = string.IsNullOrEmpty(direction) ? "DESC" : direction;
            query = query.OrderBy(sortBy + " " + direction);
        }

        if (start.HasValue)
        {
            query = query.Skip(start.Value);
        }

        if (pageSize.HasValue)
        {
            query = query.Take(pageSize.Value);
        }

        return await query
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.CountAsync();
    }
}
