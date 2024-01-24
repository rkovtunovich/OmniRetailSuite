namespace BackOffice.Client.Services;

public class CacheEntry<T>
{
    public CacheEntry(T item)
    {
        Value = item;
    }

    public T Value { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
