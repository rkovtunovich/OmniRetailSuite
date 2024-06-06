namespace Retail.Data.Repositories;

public class ReceiptRepository(RetailDbContext context, ILogger<ReceiptRepository> logger) : IReceiptRepository
{
    private readonly RetailDbContext _context = context;
    private readonly ILogger<ReceiptRepository> _logger = logger;

    public async Task<List<Receipt>> GetReceiptsAsync()
    {
        try
        {
            return await _context.Receipts
                .Select(r => new Receipt
                {
                    Id = r.Id,
                    CodeNumber = r.CodeNumber,
                    CodePrefix = r.CodePrefix,
                    Date = r.Date,
                    Cashier = new Cashier
                    {
                        Id = r.Cashier.Id,
                        Name = r.Cashier.Name,
                        CodeNumber = r.Cashier.CodeNumber,
                    },
                    Store = new Store
                    {
                        Id = r.Store.Id,
                        Name = r.Store.Name,
                        CodeNumber = r.Store.CodeNumber,
                    },
                    TotalPrice = r.TotalPrice
                })
                .ToListAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting receipts");
            throw;
        }
    }

    public async Task<Receipt?> GetReceiptByNumberAsync(int code, string? prefix)
    {
        try
        {
            return await _context.Receipts
                .Include(cashier => cashier.Cashier)
                .Include(store => store.Store)
                .Include(items => items.ReceiptItems!)
                    .ThenInclude(p => p.ProductItem)
                .FirstOrDefaultAsync(r => r.CodeNumber == code && r.CodePrefix == prefix);
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting receipt with number {code}{prefix}", code, prefix);
            throw;
        }
    }

    public async Task<Receipt?> GetReceiptAsync(Guid receiptId)
    {
        try
        {
            return await _context.Receipts
                .Include(items => items.ReceiptItems!)
                    .ThenInclude(p => p.ProductItem)
                .FirstOrDefaultAsync(r => r.Id == receiptId);
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting receipt with id {Id}", receiptId);
            throw;
        }
    }

    public async Task<Receipt> AddReceiptAsync(Receipt receipt)
    {
        try
        {
            _context.Receipts.Add(receipt);

            await _context.SaveChangesAsync();

            return receipt;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding receipt with id {Id}", receipt.Id);
            throw;
        }
    }

    public async Task UpdateReceiptAsync(Receipt receipt)
    {
        try
        {
            _context.Receipts.Update(receipt);

            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while updating receipt with id {Id}", receipt.Id);
            throw;
        }
    }

    public async Task DeleteReceiptAsync(Guid receiptId, bool isSoftDeleting)
    {
        try
        {
            var receipt = await _context.Receipts.FirstOrDefaultAsync(r => r.Id == receiptId) ?? throw new Exception($"Receipt with id {receiptId} not found");

            if (isSoftDeleting)
            {
                receipt.IsDeleted = true;

                _context.Receipts.Update(receipt);
            }
            else
            {
                _context.Receipts.Remove(receipt);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while deleting receipt with id {Id}", receiptId);
            throw;
        }
    }
}
