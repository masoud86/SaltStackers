using SaltStackers.Domain.Models.Setting;

namespace SaltStackers.Data.Seed
{
    public partial class Seeder
    {
        public void ApplicationSetting()
        {
            _modelBuilder.Entity<ApplicationSetting>().HasData(
                new ApplicationSetting
                {
                    Key = "RoleValidationKey",
                    Value = "6804a190-255a-4ea1-9960-374de2334d9e"
                },
                new ApplicationSetting
                {
                    Key = "DefaultTimeZone",
                    Value = "Pacific Daylight Time"
                },
                new ApplicationSetting
                {
                    Key = "ReCaptchaSiteKey",
                    Value = "6LdXzIYbAAAAAOJFykyyrl_tvFm2NI1PyTqMjIMp"
                },
                new ApplicationSetting
                {
                    Key = "ReCaptchaSecretKey",
                    Value = "6LdXzIYbAAAAACDsprm4U9yvY8wFpER6QSOWqh2d"
                },
                new ApplicationSetting
                {
                    Key = "RecaptchaScoreThreshold",
                    Value = "0.5"
                }
            );

            _modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Title = "Canada",
                    IsActive = true
                }
            );

            _modelBuilder.Entity<Province>().HasData(
                new Province
                {
                    Id = 1,
                    Title = "BC - British Columbia",
                    IsActive = true,
                    CountryId = 1
                }
            );

            _modelBuilder.Entity<City>().HasData(
                new City
                {
                    Id = 1,
                    Title = "Greater Vancouver",
                    IsActive = true,
                    ProvinceId = 1
                }
            );

            _modelBuilder.Entity<Zone>().HasData(
                new Zone
                {
                    Id = 1,
                    Title = "All",
                    IsActive = true,
                    CityId = 1,
                    DeliveryPrice = (decimal)10.00
                }
            );
        }
    }
}
