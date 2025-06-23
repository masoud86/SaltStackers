using System.Reflection;
using System.Text.RegularExpressions;

namespace SaltStackers.Common.Helper
{
    public static class TemplateHelper
    {
        static readonly Regex replacePattern = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);

        public static string GetTemplateContent(string template)
        {
            //Replace all "images/" with "{{PublicUrl}}/"
            var bodyPath = Path.Combine(Directory.GetCurrentDirectory(), $"{template}.html");
            if (File.Exists(bodyPath))
            {
                return File.ReadAllText(bodyPath);
            }
            return string.Empty;
        }

        private static Dictionary<string, string> AddExtraReplaces(this Dictionary<string, string> dic, bool testMode)
        {
            var test = testMode ? "test" : "";
            dic.Add("AppUrl", $"https://app{test}.saltstackers.com");
            dic.Add("PublicUrl", $"https://public{test}.saltstackers.com/email");
            dic.Add("ContactNumber", "+1 (604) 364-1416");
            dic.Add("CurrentYear", DateTime.Now.Year.ToString());
            return dic;
        }

        public static string GenerateEmailBody(string template, object model, bool testMode)
        {
            var body = GetTemplateContent(template);

            if (string.IsNullOrEmpty(body))
            {
                return string.Empty;
            }

            var replaceList = new Dictionary<string, string>();
            if (model != null)
            {
                foreach (var prop in model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var valueObject = prop.GetValue(model, null);
                    var value = valueObject != null ? valueObject.ToString() : string.Empty;
                    replaceList.Add(prop.Name, value);
                }
            }

            replaceList.AddExtraReplaces(testMode);

            return replacePattern.Replace(body, match => replaceList[match.Groups[1].Value]);
        }
    }
}
