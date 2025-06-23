using System.Globalization;

namespace SaltStackers.Web.Helpers
{
    public static class CultureHelper
    {
        public const string DefaultCulture = "en-US";

        public static void SetCulture(string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
