namespace Shared.Core.Abstraction;

public interface ICodedEntity
{
    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; }
}
