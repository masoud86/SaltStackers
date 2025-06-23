using SaltStackers.Domain.Models.Message;
using System.Threading.Tasks;

namespace SaltStackers.Domain.Interfaces
{
    public interface IMessageRepository
    {
        EmailGateway GetDefaultEmailGateway();

        Task<SmsGateway> GetSmsGateway(string name);
    }
}
