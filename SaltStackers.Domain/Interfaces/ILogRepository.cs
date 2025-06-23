using SaltStackers.Domain.Models.Log;
using System.Linq.Expressions;

namespace SaltStackers.Domain.Interfaces
{
    public interface ILogRepository
    {
        Task AddApplicationLogAsync(ApplicationLog model);

        Task AddUserActivityLogAsync(UserActivityLog model);

        Task<int> GetUserActivityLogsCount(Expression<Func<UserActivityLog, bool>> predicate = null);

        Task<List<UserActivityLog>> GetUserActivitiesLogAsync(int start, int pageSize, string sortBy, string direction,
            Expression<Func<UserActivityLog, bool>> predicate = null);

        Task<UserActivityLog> GetUserActivityLogByIdAsync(Guid id);

        Task<int> GetApplicationLogsCountAsync(Expression<Func<ApplicationLog, bool>> predicate = null);

        Task<List<ApplicationLog>> GetApplicationLogsAsync(int start, int pageSize, string sortBy, string direction,
            Expression<Func<ApplicationLog, bool>> predicate = null);
    }
}
