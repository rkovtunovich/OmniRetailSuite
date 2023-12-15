using BackOffice.Core.Models.Retail;
using Contracts.Dtos.Retail;

namespace BackOffice.Application.Mapping.Retail;

public static class CashierMapping
{
    public static Cashier ToModel(this CashierDto cashierDto)
    {
        return new Cashier
        {
            Id = cashierDto.Id,
            Name = cashierDto.Name,
            CodeNumber = cashierDto.CodeNumber,
            CodePrefix = cashierDto?.CodePrefix
        };
    }

    public static CashierDto ToDto(this Cashier cashier)
    {
        return new CashierDto
        {
            Id = cashier.Id,
            Name = cashier.Name,
            CodeNumber = cashier.CodeNumber,
            CodePrefix = cashier?.CodePrefix
        };
    }
}
