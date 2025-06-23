using System.Threading.Tasks;
using SaltStackers.Application.ViewModels.WebRequest;

namespace SaltStackers.Application.Interfaces
{
    public interface IWebRequestService
    {
        Task<T> CallAsync<T>(ApiModel api, object parameters, string baseUrl = "", string publicKey = "", string privateKey = "", string receiptNumber = null, string requestNumber = null, string userId = null);
    }
}
