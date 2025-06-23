using Microsoft.AspNetCore.Http;
using SaltStackers.Application.ViewModels.Log;

namespace SaltStackers.Application.Interfaces
{
    public interface ILogService
    {
        Task AddUserLogAsync(UserActivityLogDto model);

        Task AddUserLogAsync(string userId, string type, string descriptionKey, string descriptionParameters, ClientInformation client, string receiptNumber = null, string requestNumber = null);

        Task AddUserLogAsync(string userId, string type, string descriptionKey, string descriptionParameters, string actionRelatedId, ClientInformation client, string receiptNumber = null, string requestNumber = null);

        Task AddUserLogAsync(string userId, string type, string descriptionKey, string descriptionParameters, object content, ClientInformation client, string receiptNumber = null, string requestNumber = null);

        Task AddUserLogAsync(string userId, string type, string descriptionKey, string descriptionParameters, string actionRelatedId, object content, ClientInformation client, string receiptNumber = null, string requestNumber = null);

        Task<List<UserActivityLogDto>> GetUserActivityLogAsync(UserActivityLogFilters filter);

        Task<UserActivityLogs> GetUserActivityLogModelAsync(UserActivityLogFilters filter);

        Task<UserActivityLogDto> GetUserActivityLogById(Guid id);

        Task<ApplicationLogs> GetApplicationLogModelAsync(ApplicationLogFilters filter);
    }
}
