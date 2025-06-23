using Microsoft.EntityFrameworkCore;
using SaltStackers.Data.Context;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Financial;

namespace SaltStackers.Data.Repository
{
    public class FinancialRepository : IFinancialRepository
    {
        private readonly AppDbContext _context;

        public FinancialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaxProfile>?> GetTaxProfilesAsync()
        {
            return await _context.TaxProfiles.ToListAsync();
        }
    }
}
