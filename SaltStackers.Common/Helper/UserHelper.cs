using System.Text.RegularExpressions;

namespace SaltStackers.Common.Helper
{
    public static class UserHelper
    {
        public static string UsernameGenerator(string name, string email)
        {
            var rgx = new Regex("[^a-zA-Z]");
            var fullName = rgx.Replace(name.ToLower().Trim(), "");
            var emailName = rgx.Replace(email.ToLower().Split('@')[0], "").ToString();

            string? username;
            if (!string.IsNullOrEmpty(fullName) && fullName.Length >= 4)
            {
                username = fullName;
            }
            else if (!string.IsNullOrEmpty(emailName) && emailName.Length >= 4)
            {
                username = emailName;
            }
            else
            {
                username = StringHelper.RandomString(4, true);
            }


            return $"{username[..4]}{new Random().Next(1000, 9999)}";
        }
    }
}
