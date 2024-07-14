namespace Helpers.DateTimeHelper;

public static class Converter
{
    public static DateTime UtcToLocalDateTime(DateTimeOffset utcDateTime)
    {
        return utcDateTime.ToLocalTime().DateTime;
    }
}
