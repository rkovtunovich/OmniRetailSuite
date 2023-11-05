using Retail.Core.DTOs;

namespace Retail.Application.Services.Abstraction;

public interface IReceiptService
{
    Task<ReceiptDto?> GetReceiptAsync(int id);

    Task<List<ReceiptDto>> GetReceiptsAsync();

    Task<ReceiptDto> CreateReceiptAsync(ReceiptDto receiptDto);

    Task<ReceiptDto> UpdateReceiptAsync(ReceiptDto receiptDto);

    Task DeleteReceiptAsync(int id, bool isSoftDeleting);
}
