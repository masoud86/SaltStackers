namespace SaltStackers.Web.Helpers
{
    public static class ConfigHelper
    {
        public static string CurrentVersion()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            return root.GetSection("Version").Get<string>();
        }
    }
}
