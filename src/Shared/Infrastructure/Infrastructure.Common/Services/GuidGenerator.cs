namespace Infrastructure.Common.Services;

public class GuidGenerator : IGuidGenerator
{
    public Guid Create() => Guid.NewGuid();
}
