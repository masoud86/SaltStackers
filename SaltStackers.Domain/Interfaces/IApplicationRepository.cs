using SaltStackers.Domain.Models.Setting;

namespace SaltStackers.Domain.Interfaces
{
    public interface IApplicationRepository
    {
        Task<List<ApplicationSetting>> GetApplicationSettingsAsync();

        Task SetApplicationSettingsAsync(ApplicationSetting model);

        void UpdateApplicationSettings(ApplicationSetting model);

        Task<List<Country>> GetActiveCountriesAsync();

        Task<List<Province>> GetActiveProvincesAsync(int countryId);

        Task<List<City>> GetActiveCitiesAsync(int provinceId);

        Task<List<Zone>> GetActiveZonesAsync(int cityId);

        Task<List<Zone>> GetActiveZonesAsync();

        Task<List<Zone>> GetZonesAsync();

        void UpdateZoneAsync(Zone model);

        Task<Zone> GetZoneAsync(int id);

        Task<List<Alert>> GetAlertsAsync(string? userId = null);

        Task SeenAlertAsync(string userId, int alertId);
    }
}
