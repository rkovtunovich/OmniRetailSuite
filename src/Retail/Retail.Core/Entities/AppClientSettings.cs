namespace Retail.Core.Entities;

public class AppClientSettings: EntityBase
{
    public Guid StoreId { get; set; }

    public Store Store { get; set; } = null!;
}
