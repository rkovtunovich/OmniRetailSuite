namespace BackOffice.Core.Models.UserPreferences;

public class Settings
{
    public Language Language { get; set; }

    public string Theme { get; set; } = null!;

    public bool IsDarkMode => Theme == "dark";
}
