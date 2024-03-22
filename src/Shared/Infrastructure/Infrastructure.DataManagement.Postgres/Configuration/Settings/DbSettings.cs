namespace Infrastructure.DataManagement.Postgres.Configuration.Settings;

public class DbSettings
{
    public const string SectionName = "db";

    public string Host { get; set; } = null!;

    public int Port { get; set; }

    public string Database { get; set; } = null!;
}
