namespace BackOffice.Core.Models;

public abstract class EntityModelBase
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; }

    public string GetCode()
    {
        return $"{CodePrefix}{CodeNumber:0000}";
    }
}
