using System.Reflection;

namespace SaltStackers.Common.Helper;

public static class CustomAttributeValues
{
    public static string? GetIcon(this Enum enumValue)
    {
        Type enumType = enumValue.GetType();
        MemberInfo memberInfo = enumType.GetMember(enumValue.ToString())[0];
        var attribute = (IconAttribute?)Attribute.GetCustomAttribute(memberInfo, typeof(IconAttribute));
        return attribute?.Name;
    }
}
