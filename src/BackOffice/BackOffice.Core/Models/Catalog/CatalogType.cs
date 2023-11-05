namespace BackOffice.Core.Models.Catalog;

public class CatalogType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public override string? ToString()
    {
        return Name;
    }

    public override bool Equals(object? o)
    {
        var other = o as CatalogType;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id;
}
