namespace Infrastructure.Messaging.Kafka.Configuration.Settings;

public class KafkaSettings
{
    public const string SectionName = "Kafka";

    public string BootstrapServers { get; set; } = null!;

    public List<ProducerSettings> Producers { get; set; } = [];

    public List<ConsumerSettings> Consumers { get; set; } = [];
}
