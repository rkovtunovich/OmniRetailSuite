namespace BackOffice.Core.Models.ProductCatalog;

public class ProductParent : EntityModelBase
{
    public ProductParent? Parent { get; set; }

    public Guid? ParentId { get; set; }

    public List<ProductParent>? Children { get; set; }
}
