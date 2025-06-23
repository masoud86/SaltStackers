using SaltStackers.Data.Context;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Message;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace SaltStackers.Data.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public EmailGateway GetDefaultEmailGateway()
        {
            return _context.EmailGateways.FirstOrDefault();
        }

        public async Task<SmsGateway> GetSmsGateway(string name)
        {
            return await _context.SmsGateways
                .FirstOrDefaultAsync(p => p.Name.ToUpper() == name.ToUpper().Trim());
        }
    }
}
