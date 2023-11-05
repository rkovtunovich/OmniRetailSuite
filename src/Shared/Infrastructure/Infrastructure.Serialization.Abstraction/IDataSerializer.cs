namespace Infrastructure.Serialization.Abstraction;

public interface IDataSerializer
{
    string Serialize<T>(T obj);

    string Serialize(object obj, Type type);

    T? Deserialize<T>(string obj);

    object? Deserialize(string obj, Type type);
}
