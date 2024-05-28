using Core.Abstraction;

namespace BackOffice.Core.Models.ProductCatalog;

public class ProductType : EntityModelBase, ICloneable<ProductType>
{
    public override string? ToString()
    {
        return Name;
    }

    public override bool Equals(object? o)
    {
        var other = o as ProductType;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public ProductType Clone()
    {
        return new ProductType
        {
            Name = $"{Name} (Copy)",
        };
    }
}
