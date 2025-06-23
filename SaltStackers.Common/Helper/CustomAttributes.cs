namespace SaltStackers.Common.Helper;

[AttributeUsage(AttributeTargets.Field)]
public class IconAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}
