using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class CashierService(IRetailRepository<Cashier> cashierRepository, IMapper mapper, ILogger<CashierService> logger) : ICashierService
{
    public async Task<List<CashierDto>> GetCashiersAsync()
    {
        try
        {
            var cashiers = await cashierRepository.GetEntitiesAsync();
            return mapper.Map<List<CashierDto>>(cashiers);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while getting cashiers");
            throw;
        }
    }

    public async Task<CashierDto?> GetCashierAsync(Guid id)
    {
        try
        {
            var cashier = await cashierRepository.GetEntityAsync(id);

            return mapper.Map<CashierDto>(cashier);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while getting cashier: id {id}");
            throw;
        }
    }

    public async Task<CashierDto> CreateCashierAsync(CashierDto cashierDto)
    {
        var cashier = mapper.Map<Cashier>(cashierDto);
        await cashierRepository.AddEntityAsync(cashier);
        return mapper.Map<CashierDto>(cashier);
    }

    public async Task<CashierDto> UpdateCashierAsync(CashierDto cashierDto)
    {
        try
        {
            var cashier = await cashierRepository.GetEntityAsync(cashierDto.Id) ?? throw new Exception($"Cashier with id {cashierDto.Id} not found");
            cashier.Name = cashierDto.Name;

            await cashierRepository.UpdateEntityAsync(cashier);

            return mapper.Map<CashierDto>(cashier);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while updating cashier: id {cashierDto.Id}");
            throw;
        }
    }

    public async Task DeleteCashierAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            await cashierRepository.DeleteEntityAsync(id, isSoftDeleting);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while deleting cashier: id {id}");
            throw;
        }
    }
}
