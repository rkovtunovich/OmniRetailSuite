namespace BackOffice.Core.Models.Product;

public class Brand
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public override string? ToString()
    {
        return Name;
    }

    public override bool Equals(object? o)
    {
        var other = o as Brand;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
