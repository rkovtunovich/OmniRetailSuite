namespace RetailAssistant.Core.Models.Retail;

public class Store : EntityModelBase
{
    public string Address { get; set; } = null!;

    public List<Cashier> Cashiers { get; set; } = [];
}
