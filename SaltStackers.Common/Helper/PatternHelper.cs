namespace SaltStackers.Common.Helper
{
    public static class PatternHelper
    {
        public const string Dollar = "^(?=.)\\d{0,6}(\\.\\d{1,2})?$";

        public const string DangerousCharacters = "^[^<>$@*!#]+$";
        public const string DangerousCharactersSimplify = "^[^<>$@*!]+$";

        public const string PasswordPattern = "(?=^.{8,}$)((?=.*\\d)|(?=.*\\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$";

        //public const string PhoneNumberPattern = @"^\d{10}|(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$";
        public const string PhoneNumberPattern = @"^\d{10}$";

        public static string GenerateCode(this string prefix)
        {
            return $"{prefix}{new Random().Next(1000000, 9999999)}";
        }
    }


}
