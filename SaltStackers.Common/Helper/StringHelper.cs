using System.Text;

namespace SaltStackers.Common.Helper;

public static class StringHelper
{
    public static string RemoveFirstWord(this string input)
    {
        if (input.Length > 0)
        {
            int index = input.IndexOf(" ") + 1;
            return input.Substring(index);
        }
        return input;
    }

    public static string RandomString(int size, bool lowerCase = false)
    {
        var builder = new StringBuilder(size);
        char offset = lowerCase ? 'a' : 'A';
        const int lettersOffset = 26;

        for (var i = 0; i < size; i++)
        {
            var @char = (char)new Random().Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }

        return lowerCase ? builder.ToString().ToLower() : builder.ToString();
    }
}
