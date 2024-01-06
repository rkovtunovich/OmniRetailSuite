namespace BackOffice.Core.Models.Retail;

public class Store : EntityModelBase
{
    public List<Cashier> Cashiers { get; set; } = [];
}
