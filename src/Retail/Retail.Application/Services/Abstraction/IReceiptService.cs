namespace Retail.Application.Services.Abstraction;

public interface IReceiptService
{
    Task<ReceiptDto?> GetReceiptAsync(Guid id);

    Task<List<ReceiptDto>> GetReceiptsAsync();

    Task<ReceiptDto> CreateReceiptAsync(ReceiptDto receiptDto);

    Task<ReceiptDto> UpdateReceiptAsync(ReceiptDto receiptDto);

    Task DeleteReceiptAsync(Guid id, bool isSoftDeleting);
}
