using Retail.Core.DTOs;

namespace Retail.Application.Services.Abstraction;

public interface ICashierService
{
    Task<CashierDto?> GetCashierAsync(int id);

    Task<List<CashierDto>> GetCashiersAsync();

    Task<CashierDto> CreateCashierAsync(CashierDto cashierDto);

    Task<CashierDto> UpdateCashierAsync(CashierDto cashierDto);

    Task DeleteCashierAsync(int id, bool isSoftDeleting);
}
