namespace BackOffice.Core.Models.UserPreferences;

public class Settings
{
    public string Language { get; set; } = null!;

    public string Theme { get; set; } = null!;

    public bool IsDarkMode => Theme == "dark";
}
