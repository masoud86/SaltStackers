using System.Resources;

namespace SaltStackers.Common.Helper
{
    public static class CultureHelper
    {
        public const string DefaultCulture = "en-US";

        public static string CurrentCulture => Thread.CurrentThread.CurrentCulture.Name;

        public static string GetResource(this string key, Type resource, bool noParam)
        {
            return key.GetResource(resource);
        }

        public static string GetResource(this string key, Type resource, string parameters)
        {
            string[] parametersArray = null;
            if (!string.IsNullOrEmpty(parameters))
            {
                parametersArray = parameters.Split(',');
            }
            return key.GetResource(resource, parametersArray);
        }

        public static string GetResource(this string key, Type resource, string[] parameters = null)
        {
            var resourceManagement = new ResourceManager(resource);
            if (parameters == null)
            {
                return resourceManagement.GetString(key);
            }
            return string.Format(resourceManagement.GetString(key) ?? string.Empty, parameters);
        }
    }
}
