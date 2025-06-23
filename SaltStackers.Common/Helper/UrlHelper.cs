using System.Web;

namespace SaltStackers.Common.Helper
{
    public static class UrlHelper
    {
        public static string ConvertObjectQuerystring(this object obj)
        {
            var properties = obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null)
                .Select(p => p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null)?.ToString()));

            return string.Join("&", properties.ToArray());
        }

        public static string Base64UrlEncode(string input)
        {
            char[] padding = { '=' };
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
                .TrimEnd(padding)
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
