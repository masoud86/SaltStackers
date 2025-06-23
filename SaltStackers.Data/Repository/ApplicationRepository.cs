using SaltStackers.Data.Context;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Setting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaltStackers.Data.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationSetting>> GetApplicationSettingsAsync()
        {
            //Extra check to avoid error during create database (ReCaptcha configuration in SecurityServices class)
            if (_context != null && _context.Database.CanConnect())
            {
                return await _context.ApplicationSettings.ToListAsync();
            }
            return null;
        }

        public async Task SetApplicationSettingsAsync(ApplicationSetting model)
        {
            await _context.ApplicationSettings.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public void UpdateApplicationSettings(ApplicationSetting model)
        {
            _context.ApplicationSettings.Update(model);
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<List<Country>> GetActiveCountriesAsync()
        {
            return await _context.Countries
                .Where(p => p.IsActive)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<List<Province>> GetActiveProvincesAsync(int countryId)
        {
            return await _context.Provinces
                .Where(p => p.CountryId == countryId && p.IsActive)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<List<City>> GetActiveCitiesAsync(int provinceId)
        {
            return await _context.Cities
                .Where(p => p.ProvinceId == provinceId && p.IsActive)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<List<Zone>> GetActiveZonesAsync(int cityId)
        {
            return await _context.Zones
                .Include(p => p.City)
                .Where(p => p.CityId == cityId && p.IsActive)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<List<Zone>> GetActiveZonesAsync()
        {
            return await _context.Zones
                .Include(p => p.City)
                .Where(p => p.IsActive)
                .OrderBy(p => p.City.Title)
                .ThenBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<List<Zone>> GetZonesAsync()
        {
            return await _context.Zones
                .ToListAsync();
        }

        public void UpdateZoneAsync(Zone model)
        {
            var zone = _context.Zones.Find(model.Id);
            if (zone != null)
            {
                zone.EditDateTime = DateTime.UtcNow;
                _context.Zones.Update(zone);
                _context.Entry(zone).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public async Task<Zone> GetZoneAsync(int id)
        {
            return await _context.Zones.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Alert>> GetAlertsAsync(string? userId = null)
        {
            return await _context.Alerts
                .Include(p => p.AlertUsers)
                .Where(p => p.IsActive &&
                    (!p.StartDateTime.HasValue || p.StartDateTime.Value < DateTime.UtcNow) &&
                    (!p.EndDateTime.HasValue || p.EndDateTime.Value > DateTime.UtcNow) &&
                    (p.IsPublic || p.AlertUsers.Any(q => q.UserId == userId && !q.IsSeen)))
                .OrderBy(p => p.StartDateTime).ThenBy(p => p.CreateDateTime)
                .ToListAsync();
        }

        public async Task SeenAlertAsync(string userId, int alertId)
        {
            var alertUser = await _context.AlertUsers
                .FirstOrDefaultAsync(p => p.UserId == userId && p.AlertId == alertId);

            if (alertUser != null)
            {
                alertUser.IsSeen = true;
                alertUser.ViewDateTime = DateTime.UtcNow;
                _context.AlertUsers.Update(alertUser);
                _context.Entry(alertUser).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
