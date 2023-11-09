using Shared.Core.Abstraction;

namespace Retail.Core.Entities.ReceiptAggregate;

public class CatalogItem: BaseEntity
{
    public string Name { get; set; } = null!;
}
