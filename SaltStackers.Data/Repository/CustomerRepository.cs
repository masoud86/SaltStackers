using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SaltStackers.Data.Context;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Membership;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace SaltStackers.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCustomersCountAsync(Expression<Func<AspNetUser, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _context.AspNetUsers.CountAsync();
            }
            return await _context.AspNetUsers
                .CountAsync(predicate);
        }

        public async Task<List<AspNetUser>> GetCustomersAsync(int start, int pageSize, string sortBy, string direction, Expression<Func<AspNetUser, bool>> predicate = null)
        {
            return await _context.AspNetUsers
                .Where(predicate)
                .OrderBy(sortBy + " " + direction)
                .Skip(start).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AspNetUser> FindUserByPhoneNumber(string phoneNumber)
        {
            return await _context.AspNetUsers
                .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
        }

        public async Task<AspNetUser> FindCustomerByEmailAsync(string emailAddress)
        {
            return await _context.AspNetUsers
                .FirstOrDefaultAsync(p => p.NormalizedEmail == emailAddress);
        }

        public async Task<bool> CreateCustomerAsync(AspNetUser user)
        {
            var done = false;
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var role = await _context.AspNetRoles.FirstOrDefaultAsync(p => p.Name == "Customer");
                    if (role != null)
                    {
                        await _context.AspNetUsers.AddAsync(user);
                        await _context.AspNetUserRoles.AddAsync(new IdentityUserRole<string>
                        {
                            UserId = user.Id,
                            RoleId = role.Id
                        });

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        done = true;
                    }
                }
                catch
                {
                    await transaction.RollbackAsync();
                }
            });
            return done;
        }
    }
}
