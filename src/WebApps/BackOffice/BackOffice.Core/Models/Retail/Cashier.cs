namespace BackOffice.Core.Models.Retail;

public class Cashier : EntityModelBase, ICloneable<Cashier>
{
    public Cashier Clone()
    {
        return new Cashier
        {
            Name = $"{Name} (Copy)",
        };
    }
}
