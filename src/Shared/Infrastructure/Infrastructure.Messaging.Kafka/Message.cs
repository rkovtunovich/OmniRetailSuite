namespace Infrastructure.Messaging.Kafka;

public record Message(string DataType, string Payload)
{
}
