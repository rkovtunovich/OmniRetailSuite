using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Core.Repositories;

public interface IReceiptRepository
{
    Task<List<Receipt>> GetReceiptsAsync();

    Task<Receipt?> GetReceiptAsync(int receiptId);

    Task<Receipt?> GetReceiptAsync(string receiptNumber);

    Task<Receipt> AddReceiptAsync(Receipt receipt);

    Task UpdateReceiptAsync(Receipt receipt);

    Task DeleteReceiptAsync(int receiptId, bool useSoftDeleting);
}
