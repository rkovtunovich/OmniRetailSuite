using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class CashierService: ICashierService
{
    private readonly IRetailRepository<Cashier> _cashierRepository;
    private readonly ILogger<CashierService> _logger;

    public CashierService(IRetailRepository<Cashier> cashierRepository, ILogger<CashierService> logger)
    {
        _cashierRepository = cashierRepository;
        _logger = logger;
    }

    public async Task<List<CashierDto>> GetCashiersAsync()
    {
        try
        {
            var cashiers = await _cashierRepository.GetEntitiesAsync();
            return cashiers.Select(cashier => cashier.ToDto()).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting cashiers");
            throw;
        }
    }

    public async Task<CashierDto?> GetCashierAsync(Guid id)
    {
        try
        {
            var cashier = await _cashierRepository.GetEntityAsync(id);

            return cashier?.ToDto();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while getting cashier: id {id}");
            throw;
        }
    }

    public async Task<CashierDto> CreateCashierAsync(CashierDto cashierDto)
    {
        var cashier = cashierDto.ToEntity();
        await _cashierRepository.AddEntityAsync(cashier);
        return cashier.ToDto();
    }

    public async Task<CashierDto> UpdateCashierAsync(CashierDto cashierDto)
    {
        try
        {
            var cashier = await _cashierRepository.GetEntityAsync(cashierDto.Id) ?? throw new Exception($"Cashier with id {cashierDto.Id} not found");
            cashier.Name = cashierDto.Name;

            await _cashierRepository.UpdateEntityAsync(cashier);

            return cashier.ToDto();
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
            await _cashierRepository.DeleteEntityAsync(id, isSoftDeleting);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while deleting cashier: id {id}");
            throw;
        }
    }
}
