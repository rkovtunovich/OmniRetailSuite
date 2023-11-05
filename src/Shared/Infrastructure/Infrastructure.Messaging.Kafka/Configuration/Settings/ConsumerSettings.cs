namespace Infrastructure.Messaging.Kafka.Configuration.Settings;

public class ConsumerSettings
{
    public string Name { get; set; } = null!;

    public string Topic { get; set; } = null!;

    public string GroupId { get; set; } = null!;
}
