namespace RetailAssistant.Core.Models.UserPreferences;

public class UserPreference
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public Settings Settings { get; set; } = null!;

    public DateTimeOffset UpdatedAt { get; set; }
}
