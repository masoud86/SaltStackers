using Newtonsoft.Json;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Log;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Log;

namespace SaltStackers.Application.Services
{
    public class LoggerService<T> : ILoggerService<T> where T : class
    {
        private readonly ILogRepository _logRepository;

        public LoggerService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task Log(ApplicationLogType type, string message, object logObject = null, string receiptNumber = null, string requestNumber = null, string groupKey = null, string userId = null)
        {
            switch (type)
            {
                case ApplicationLogType.Info:
                    await LogInfo(message, logObject, receiptNumber, requestNumber, groupKey, userId);
                    break;
                case ApplicationLogType.Warning:
                    await LogWarning(message, logObject, receiptNumber, requestNumber, groupKey, userId);
                    break;
                case ApplicationLogType.Error:
                    await LogError(message, logObject, receiptNumber, requestNumber, groupKey, userId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private async Task AddLog(string type, string message, object logObject = null, string receiptNumber = null,
            string requestNumber = null, string groupKey = null, string userId = null)
        {
            await _logRepository.AddApplicationLogAsync(new ApplicationLog
            {
                Id = Guid.NewGuid(),
                Level = type,
                Message = message,
                Logger = typeof(T).FullName,
                ReceiptNumber = receiptNumber,
                RequestNumber = requestNumber,
                Parameters = JsonConvert.SerializeObject(logObject),
                GroupKey = groupKey,
                UserId = userId
            });
        }

        public async Task LogError(string message, object logObject = null, string receiptNumber = null, string requestNumber = null, string groupKey = null, string userId = null)
        {
            await AddLog(ApplicationLogType.Error.ToString(),
                message, logObject, receiptNumber, requestNumber, groupKey, userId);
        }

        public async Task LogInfo(string message, object logObject = null, string receiptNumber = null, string requestNumber = null, string groupKey = null, string userId = null)
        {
            await AddLog(ApplicationLogType.Info.ToString(),
                message, logObject, receiptNumber, requestNumber, groupKey, userId);
        }

        public async Task LogWarning(string message, object logObject = null, string receiptNumber = null, string requestNumber = null, string groupKey = null, string userId = null)
        {
            await AddLog(ApplicationLogType.Warning.ToString(),
                message, logObject, receiptNumber, requestNumber, groupKey, userId);
        }
    }
}
