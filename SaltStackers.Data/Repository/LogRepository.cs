using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SaltStackers.Data.Context;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Log;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace SaltStackers.Data.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly AppDbContext _context;
        private readonly IServiceScopeFactory _scopeFactory;

        public LogRepository(AppDbContext context, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _scopeFactory = scopeFactory;
        }

        public async Task AddApplicationLogAsync(ApplicationLog model)
        {
            try
            {
                await _context.ApplicationLogs.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            catch
            {
                //Ignored
            }
        }

        public async Task AddUserActivityLogAsync(UserActivityLog model)
        {
            try
            {
                await _context.UserActivitiesLogs.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            catch
            {
                //Ignored
            }
        }

        public async Task<int> GetUserActivityLogsCount(Expression<Func<UserActivityLog, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.UserActivitiesLogs.CountAsync();
            }
            return await _context.UserActivitiesLogs
                .CountAsync(predicate);
        }

        public async Task<List<UserActivityLog>> GetUserActivitiesLogAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<UserActivityLog, bool>> predicate = null)
        {
            return await _context.UserActivitiesLogs
                .Where(predicate)
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UserActivityLog> GetUserActivityLogByIdAsync(Guid id)
        {
            return await _context.UserActivitiesLogs
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetApplicationLogsCountAsync(Expression<Func<ApplicationLog, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.ApplicationLogs.CountAsync();
            }
            return await _context.ApplicationLogs
                .CountAsync(predicate);
        }

        public async Task<List<ApplicationLog>> GetApplicationLogsAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<ApplicationLog, bool>> predicate = null)
        {
            return await _context.ApplicationLogs
                .Where(predicate)
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
