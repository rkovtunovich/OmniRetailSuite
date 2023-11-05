
namespace BackOffice.Core.Models.Catalog;

public class CatalogBrand 
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public override string? ToString()
    {
        return Name;
    }

    public override bool Equals(object? o)
    {
        var other = o as CatalogBrand;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id;
}
