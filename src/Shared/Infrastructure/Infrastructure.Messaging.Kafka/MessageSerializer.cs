using System.Text;
using Confluent.Kafka;
using Infrastructure.Serialization.Abstraction;

namespace Infrastructure.Messaging.Kafka;

public class MessageSerializer : ISerializer<Message>, IDeserializer<Message>
{
    private readonly IDataSerializer _dataSerializer;

    public MessageSerializer(IDataSerializer dataSerializer)
    {
        _dataSerializer = dataSerializer;
    }

    public byte[] Serialize(Message data, SerializationContext context)
    {
        var json = _dataSerializer.Serialize(data);
        return Encoding.UTF8.GetBytes(json);
    }

    public Message Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            throw new ArgumentNullException(nameof(data));    
            
        var json = Encoding.UTF8.GetString(data.ToArray());

        return _dataSerializer.Deserialize<Message>(json) ?? throw new NullReferenceException(nameof(json));
    }
}
