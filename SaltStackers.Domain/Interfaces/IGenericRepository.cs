using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace SaltStackers.Domain.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> FirstAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        Expression<Func<object, TEntity>>? select = null);

    Task<List<TEntity>> GetAsync(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<object, TEntity>>? select = null,
        int? start = null, int? pageSize = null,
        string? sortBy = null, string? direction = null);

    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
}
