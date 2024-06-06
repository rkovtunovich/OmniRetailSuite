namespace BackOffice.Core.Models.Retail;

public class Store : EntityModelBase, ICloneable<Store>
{
    public string Address { get; set; } = null!;

    public List<Cashier> Cashiers { get; set; } = [];

    public Store Clone()
    {
        return new Store
        {
            Name = $"{Name} (Copy)",
            Address = Address
        };
    }

    public override string ToString()
    {
        return Name;
    }
}
