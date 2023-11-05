using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Core.DTOs;

public record CashierDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public static CashierDto FromCashier(Cashier cashier)
    {
        return new CashierDto
        {
            Id = cashier.Id,
            Name = cashier.Name
        };
    }

    public static List<CashierDto> FromCashiers(List<Cashier> cashiers)
    {
        return cashiers.Select(FromCashier).ToList();
    }

    public Cashier ToCashier()
    {
        return new Cashier
        {
            Id = Id,
            Name = Name
        };
    }
}
