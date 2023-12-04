using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Core.Repositories;

public interface IReceiptRepository
{
    Task<List<Receipt>> GetReceiptsAsync();

    Task<Receipt?> GetReceiptAsync(Guid receiptId);

    Task<Receipt?> GetReceiptByNumberAsync(int code, string? prefix);

    Task<Receipt> AddReceiptAsync(Receipt receipt);

    Task UpdateReceiptAsync(Receipt receipt);

    Task DeleteReceiptAsync(Guid receiptId, bool isSoftDeleting);
}
