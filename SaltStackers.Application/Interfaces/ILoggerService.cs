using SaltStackers.Application.ViewModels.Log;
using System.Threading.Tasks;

namespace SaltStackers.Application.Interfaces
{
    public interface ILoggerService<T> where T : class
    {
        Task Log(ApplicationLogType type, string message, object logObject = null, string receiptNumber = null, string requestNumber = null, string groupKey = null, string userId = null);

        Task LogInfo(string message, object logObject = null, string receiptNumber = null, string requestNumber = null, string groupKey = null, string userId = null);
        
        Task LogWarning(string message, object logObject = null, string receiptNumber = null, string requestNumber = null, string groupKey = null, string userId = null);
        
        Task LogError(string message, object logObject = null, string receiptNumber = null, string requestNumber = null, string groupKey = null, string userId = null);
    }
}
