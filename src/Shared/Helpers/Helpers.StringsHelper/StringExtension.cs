
namespace Helpers.StringsHelper;

public static class StringExtension
{
    public static string ToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return char.ToLower(value[0]) + value[1..];
    }

    public static string ToPascalCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return char.ToUpper(value[0]) + value[1..];
    }

    public static string ToSnakeCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }

    public static string ToKebabCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString())).ToLower();
    }

    public static string ToTitleCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x.ToString() : x.ToString())).ToLower();
    }

    public static string ToSentenceCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return char.ToUpper(value[0]) + value[1..];
    }
}
