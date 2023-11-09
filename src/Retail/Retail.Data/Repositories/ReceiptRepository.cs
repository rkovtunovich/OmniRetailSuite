namespace Retail.Data.Repositories;

public class ReceiptRepository(RetailDbContext context, ILogger<ReceiptRepository> logger) : IReceiptRepository
{
    private readonly RetailDbContext _context = context;
    private readonly ILogger<ReceiptRepository> _logger = logger;

    public async Task<List<Receipt>> GetReceiptsAsync()
    {
        try
        {
            return await _context.Receipts.ToListAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting receipts");
            throw;
        }
    }

    public async Task<Receipt?> GetReceiptAsync(string receiptNumber)
    {
        try
        {
            return await _context.Receipts
                .FirstOrDefaultAsync(r => r.Number == receiptNumber);
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting receipt with number {Number}", receiptNumber);
            throw;
        }
    }

    public async Task<Receipt?> GetReceiptAsync(Guid receiptId)
    {
        try
        {
            return await _context.Receipts
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
        catch (Exception)
        {
            _logger.LogError("Error while adding receipt with id {Id}", receipt.Id);
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

    public async Task DeleteReceiptAsync(Guid receiptId, bool useSoftDeleting)
    {
        try
        {
            var receipt = await _context.Receipts.FirstOrDefaultAsync(r => r.Id == receiptId) ?? throw new Exception($"Receipt with id {receiptId} not found");

            if (useSoftDeleting)
            {
                receipt.IsDeleted = true;

                _context.Receipts.Update(receipt);
            }
            else
            {
                _context.Receipts.Remove(receipt);
            }
        }
        catch (Exception)
        {
            _logger.LogError("Error while deleting receipt with id {Id}", receiptId);
            throw;
        }
    }
}
