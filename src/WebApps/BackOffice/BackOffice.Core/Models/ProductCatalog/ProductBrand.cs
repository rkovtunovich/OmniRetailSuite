namespace BackOffice.Core.Models.ProductCatalog;

public class ProductBrand : EntityModelBase, ICloneable<ProductBrand>
{
    public override string? ToString()
    {
        return Name;
    }

    public override bool Equals(object? o)
    {
        var other = o as ProductBrand;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public ProductBrand Clone()
    {
        return new ProductBrand
        {
            Name = $"{Name} (Copy)",
        };
    }
}
