using AutoMapper;
using SaltStackers.Application.Helpers;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Settings;
using SaltStackers.Application.ViewModels.Settings.Alert;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Setting;
using Microsoft.Extensions.Caching.Memory;

namespace SaltStackers.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _iMapper;

        public ApplicationService(IApplicationRepository applicationRepository, IOperationRepository operationRepository,
            IMemoryCache memoryCache)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            _iMapper = config.CreateMapper();
            _applicationRepository = applicationRepository;
            _operationRepository = operationRepository;
            _memoryCache = memoryCache;
        }

        public void UpdateCache()
        {
            var siteSettings = _applicationRepository.GetApplicationSettingsAsync().Result;
            if (siteSettings != null)
            {
                var dateTimeNow = DateTime.Now;
                var cacheDuration = 500;

                foreach (var siteSetting in siteSettings)
                {
                    _memoryCache.Remove(siteSetting.Key);
                    _memoryCache.Set(siteSetting.Key, siteSetting.Value, dateTimeNow.AddMinutes(cacheDuration));
                }
            }
        }

        public string GetSetting(string key)
        {
            if (!_memoryCache.TryGetValue(key, out _))
            {
                UpdateCache();
            }
            return _memoryCache.Get<string>(key);
        }

        public ServiceResult SetSettings(string key, string value)
        {
            var setting = GetSetting(key);
            var model = new ApplicationSetting
            {
                Key = key,
                Value = value,
                ChangeDateTime = DateTime.UtcNow
            };

            if (string.IsNullOrEmpty(setting))
            {
                _applicationRepository.SetApplicationSettingsAsync(model);
            }

            _applicationRepository.UpdateApplicationSettings(model);

            return new ServiceResult(true);
        }

        public async Task<List<CountryApi>> GetCountriesApiAsync()
        {
            var countries = await _applicationRepository.GetActiveCountriesAsync();
            return _iMapper.Map<List<CountryApi>>(countries);
        }

        public async Task<List<ProvinceApi>> GetProvincesApiAsync(int countryId)
        {
            var countries = await _applicationRepository.GetActiveProvincesAsync(countryId);
            return _iMapper.Map<List<ProvinceApi>>(countries);
        }

        public async Task<List<CityApi>> GetCitiesApiAsync(int provinceId)
        {
            var cities = await _applicationRepository.GetActiveCitiesAsync(provinceId);
            return _iMapper.Map<List<CityApi>>(cities);
        }

        public async Task<List<ZoneApi>> GetZonesApiAsync(int cityId)
        {
            var zones = await _applicationRepository.GetActiveZonesAsync(cityId);
            return _iMapper.Map<List<ZoneApi>>(zones);
        }

        public async Task<List<ZoneApi>> GetZonesApiAsync()
        {
            var zones = await _applicationRepository.GetActiveZonesAsync();
            return _iMapper.Map<List<ZoneApi>>(zones);
        }

        public async Task<List<ZoneApi>> GetZonesByKitchenAsync(int kitchenId)
        {
            var kitchen = await _operationRepository.GetKitchenAsync(kitchenId);
            var zones = await _applicationRepository.GetActiveZonesAsync(kitchen.Zone.CityId);
            return _iMapper.Map<List<ZoneApi>>(zones);
        }

        public async Task<List<ZoneDto>> GetZonesAsync()
        {
            var zones = await _applicationRepository.GetZonesAsync();
            return _iMapper.Map<List<ZoneDto>>(zones);
        }

        public async Task<ZoneDto> GetZoneAsync(int id)
        {
            var zone = await _applicationRepository.GetZoneAsync(id);
            return _iMapper.Map<ZoneDto>(zone);
        }

        public async Task<List<AlertApi>> GetAlertsAsync(string? userId = null)
        {
            var alerts = await _applicationRepository.GetAlertsAsync(userId);
            return _iMapper.Map<List<AlertApi>>(alerts);
        }

        public async Task SeenAlertAsync(string userId, int alertId)
        {
            await _applicationRepository.SeenAlertAsync(userId, alertId);
        }
    }
}
