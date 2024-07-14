namespace Core.WebApps.UserPreferences;

public class UserPreference
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public Settings Settings { get; set; } = null!;
}
