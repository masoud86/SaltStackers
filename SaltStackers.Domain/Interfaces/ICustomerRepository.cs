using SaltStackers.Domain.Models.Membership;
using System.Linq.Expressions;

namespace SaltStackers.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> GetCustomersCountAsync(Expression<Func<AspNetUser, bool>> predicate = null);

        Task<List<AspNetUser>> GetCustomersAsync(int start, int pageSize, string sortBy, string direction,
            Expression<Func<AspNetUser, bool>> predicate = null);

        Task<AspNetUser> FindUserByPhoneNumber(string phoneNumber);

        Task<AspNetUser> FindCustomerByEmailAsync(string emailAddress);

        Task<bool> CreateCustomerAsync(AspNetUser user);
    }
}
