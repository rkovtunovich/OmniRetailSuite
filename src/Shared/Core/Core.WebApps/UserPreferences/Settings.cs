namespace Core.WebApps.UserPreferences;

public class Settings
{
    public Language Language { get; set; } = Language.EN;

    public UITheme Theme { get; set; } = UITheme.Light;

    public bool IsDarkMode => Theme is UITheme.Dark;
}
