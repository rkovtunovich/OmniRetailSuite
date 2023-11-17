namespace Retail.Data.Repositories;

public class CashierRepository(RetailDbContext context, ILogger<ReceiptRepository> logger) : ICashierRepository
{
    private readonly RetailDbContext _context = context;
    private readonly ILogger<ReceiptRepository> _logger = logger;

    public async Task<List<Cashier>> GetCashiersAsync()
    {
        try
        {
            return await _context.Cashiers
                .ToListAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting cashiers");
            throw;
        }
    }

    public async Task<Cashier?> GetCashierAsync(Guid cashierId)
    {
        try
        {
            return await _context.Cashiers
                .FirstOrDefaultAsync(c => c.Id == cashierId);
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting cashier with id {Id}", cashierId);
            throw;
        }
    }

    public async Task<Cashier> AddCashierAsync(Cashier cashier)
    {
        try
        {
            _context.Cashiers.Add(cashier);

            await _context.SaveChangesAsync();

            return cashier;
        }
        catch (Exception)
        {
            _logger.LogError("Error while adding cashier with id {Id}", cashier.Id);
            throw;
        }
    }

    public async Task UpdateCashierAsync(Cashier cashier)
    {
        try
        {
            _context.Cashiers.Update(cashier);

            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while updating cashier with id {Id}", cashier.Id);
            throw;
        }
    }

    public async Task DeleteCashierAsync(Guid cashierId, bool isSoftDeleting)
    {
        try
        {
            var cashier = await _context.Cashiers.FirstOrDefaultAsync(c => c.Id == cashierId) ?? throw new Exception($"Cashier with id {cashierId} not found");

            if (isSoftDeleting)
            {
                cashier.IsDeleted = true;
                _context.Cashiers.Update(cashier);
            }
            else
            {
                _context.Cashiers.Remove(cashier);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while deleting cashier with id {Id}", cashierId);
            throw;
        }
    }
}
