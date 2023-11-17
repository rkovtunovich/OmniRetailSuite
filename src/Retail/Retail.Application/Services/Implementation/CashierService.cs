using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class CashierService: ICashierService
{
    private readonly ICashierRepository _cashierRepository;
    private readonly ILogger<CashierService> _logger;

    public CashierService(ICashierRepository cashierRepository, ILogger<CashierService> logger)
    {
        _cashierRepository = cashierRepository;
        _logger = logger;
    }

    public async Task<CashierDto?> GetCashierAsync(Guid id)
    {
        try
        {
            var cashier = await _cashierRepository.GetCashierAsync(id);

            return cashier is null ? null : CashierDto.FromCashier(cashier);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while getting cashier: id {id}");
            throw;
        }
    }

    public async Task<List<CashierDto>> GetCashiersAsync()
    {
        try
        {
            var cashiers = await _cashierRepository.GetCashiersAsync();
            return cashiers.Select(cashier => CashierDto.FromCashier(cashier)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting cashiers");
            throw;
        }
    }

    public async Task<CashierDto> CreateCashierAsync(CashierDto cashierDto)
    {
        var cashier = cashierDto.ToCashier();
        await _cashierRepository.AddCashierAsync(cashier);
        return CashierDto.FromCashier(cashier);
    }

    public async Task<CashierDto> UpdateCashierAsync(CashierDto cashierDto)
    {
        try
        {
            var cashier = await _cashierRepository.GetCashierAsync(cashierDto.Id) ?? throw new Exception($"Cashier with id {cashierDto.Id} not found");
            cashier.Name = cashierDto.Name;

            await _cashierRepository.UpdateCashierAsync(cashier);

            return CashierDto.FromCashier(cashier);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while updating cashier: id {cashierDto.Id}");
            throw;
        }
    }

    public async Task DeleteCashierAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            await _cashierRepository.DeleteCashierAsync(id, isSoftDeleting);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while deleting cashier: id {id}");
            throw;
        }
    }
}
