using AutoMapper;
using Microsoft.Extensions.Configuration;
using SaltStackers.Application.Helpers;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Financial;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Domain.Interfaces;

namespace SaltStackers.Application.Services
{
    public class FinancialService : IFinancialService
    {
        private readonly IFinancialRepository _financialRepository;
        private readonly INutritionRepository _nutritionRepository;
        private readonly IMapper _iMapper;

        public FinancialService(IFinancialRepository financialRepository, INutritionRepository nutritionRepository,
            IConfiguration configuration)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _iMapper = config.CreateMapper();

            _financialRepository = financialRepository;
            _nutritionRepository = nutritionRepository;
        }

        public async Task<List<ComboDto>> GetRecipeCombos(List<string> codes)
        {
            if (codes == null)
            {
                return new List<ComboDto>();
            }
            var combos = await _nutritionRepository.GetCombosByCodesAsync(codes);
            return _iMapper.Map<List<ComboDto>>(combos);
        }

        public async Task<List<TaxProfileDto>?> GetTaxProfilesAsync()
        {
            var taxProfiles = await _financialRepository.GetTaxProfilesAsync();
            return _iMapper.Map<List<TaxProfileDto>>(taxProfiles);
        }
    }
}
