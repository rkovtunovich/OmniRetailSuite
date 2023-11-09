namespace Retail.Application.Services.Abstraction;

public interface ICashierService
{
    Task<CashierDto?> GetCashierAsync(Guid id);

    Task<List<CashierDto>> GetCashiersAsync();

    Task<CashierDto> CreateCashierAsync(CashierDto cashierDto);

    Task<CashierDto> UpdateCashierAsync(CashierDto cashierDto);

    Task DeleteCashierAsync(Guid id, bool isSoftDeleting);
}
