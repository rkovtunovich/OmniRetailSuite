namespace BackOffice.Core.Models.ProductCatalog;

public class ProductParent
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; }

    public ProductParent? Parent { get; set; }

    public Guid? ParentId { get; set; }

    public HashSet<ProductParent>? Children { get; set; }

    public string GetCode()
    {
        return $"{CodePrefix}{CodeNumber:0000}";
    }
}
