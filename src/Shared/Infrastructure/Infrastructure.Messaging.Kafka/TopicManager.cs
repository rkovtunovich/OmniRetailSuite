using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Infrastructure.Messaging.Kafka.Configuration.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Messaging.Kafka;

public class TopicManager(IOptions<KafkaSettings> settings, ILogger<TopicManager> logger)
{
    private readonly IOptions<KafkaSettings> _settings = settings;
    private readonly ILogger<TopicManager> _logger = logger;

    public async Task EnsureTopicsCreatedAsync()
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _settings.Value.BootstrapServers }).Build();

        var topics = _settings.Value.Producers.Select(p => p.Topic).Union(_settings.Value.Consumers.Select(c => c.Topic)).Distinct();
        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
        var existingTopics = new HashSet<string>(metadata.Topics.Select(t => t.Topic));

        foreach (var topic in topics)
        {
            if (existingTopics.Contains(topic))
            {
                _logger.LogInformation($"Topic {topic} already exists");
                continue;
            }

            _logger.LogInformation($"Creating topic {topic}");
            try
            {
                await adminClient.CreateTopicsAsync([new() { Name = topic, ReplicationFactor = 1, NumPartitions = 1 }]);
            }
            catch (CreateTopicsException e)
            {
                _logger.LogError($"An error occurred creating topic {topic}: {e.Error.Reason}");
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred creating topic {topic}: {e.Message}");
            }
        }
    }
}
