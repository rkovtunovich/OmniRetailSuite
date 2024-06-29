namespace BackOffice.Client.Localization;

public static class SupportedCultures
{
    private static string[] Get()
    {
        return Enum.GetNames<Language>().Select(x => x.ToLower()).ToArray();
    }

    public static string[] All => Get();

    public static string Default => Get()[0];

}
