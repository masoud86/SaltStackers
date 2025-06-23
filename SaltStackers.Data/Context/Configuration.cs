using SaltStackers.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace SaltStackers.Data.Context
{
    public static class Configuration
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var seeder = new Seeder(modelBuilder);

            seeder.Identity(
                new List<UserSeed>
                {
                    new UserSeed
                    {
                        Id = seeder.DefaultAdminId,
                        Name = "Admin",
                        PhoneNumber = "+16593716466",
                        PhoneNumberConfirmed = true,
                        Email = "admin@saltstackers.com",
                        EmailConfirmed = true,
                        Password = "123456",
                        RoleId = seeder.AdminRoleId
                    }
                },
                new List<RoleSeed>
                {
                    new RoleSeed
                    {
                        Id = seeder.AdminRoleId,
                        Name = "Administrator",
                        DisplayName = "Administrator",
                        Description = "",
                        IsLocked = true
                    }
                }
            );

            seeder.ApplicationSetting();

            //seeder.Nutrition();
        }
    }
}
