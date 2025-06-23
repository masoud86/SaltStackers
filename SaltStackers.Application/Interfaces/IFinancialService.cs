using SaltStackers.Application.ViewModels.Financial;
using SaltStackers.Application.ViewModels.Nutrition;

namespace SaltStackers.Application.Interfaces
{
    public interface IFinancialService
    {
        Task<List<ComboDto>> GetRecipeCombos(List<string> codes);

        Task<List<TaxProfileDto>?> GetTaxProfilesAsync();
    }
}
