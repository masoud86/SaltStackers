using SaltStackers.Domain.Models.Financial;

namespace SaltStackers.Domain.Interfaces
{
    public interface IFinancialRepository
    {
        Task<List<TaxProfile>?> GetTaxProfilesAsync();
    }
}
