using Retail.Core.Entities;

namespace Retail.Application.Mapping;

public static class CashierMapping
{
    public static Cashier ToEntity(this CashierDto cashier)
        => new()
        {
            Id = cashier.Id,
            Name = cashier.Name,
            CodeNumber = cashier.CodeNumber,
            CodePrefix = cashier.CodePrefix
        };

    public static CashierDto ToDto(this Cashier cashier)
        => new()
        {
            Id = cashier.Id,
            Name = cashier.Name,
            CodeNumber = cashier.CodeNumber,
            CodePrefix = cashier.CodePrefix
        };
}
