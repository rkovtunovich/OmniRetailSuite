using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class ReceiptService : IReceiptService
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly ILogger<ReceiptService> _logger;

    public ReceiptService(IReceiptRepository receiptRepository, ILogger<ReceiptService> logger)
    {
        _receiptRepository = receiptRepository;
        _logger = logger;
    }

    public async Task<ReceiptDto?> GetReceiptAsync(Guid id)
    {
        try
        {
            var receipt = await _receiptRepository.GetReceiptAsync(id);

            return receipt?.ToDto();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while getting receipt: id {id}");
            throw;
        }
    }

    public async Task<List<ReceiptDto>> GetReceiptsAsync()
    {
        try
        {
            var receipts = await _receiptRepository.GetReceiptsAsync();
            return receipts.Select(receipt => receipt.ToDto()).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting receipts");
            throw;
        }
    }

    public async Task<ReceiptDto> CreateReceiptAsync(ReceiptDto receiptDto)
    {
        var receipt = receiptDto.ToEntity();
        await _receiptRepository.AddReceiptAsync(receipt);
        return receipt.ToDto();
    }

    public async Task<ReceiptDto> UpdateReceiptAsync(ReceiptDto receiptDto)
    {
        try
        {
            var receipt = await _receiptRepository.GetReceiptAsync(receiptDto.Id) ?? throw new Exception($"Receipt with id {receiptDto.Id} not found");
            receipt.Date = receiptDto.Date;
            receipt.CodeNumber = receiptDto.CodeNumber;
            receipt.CodePrefix = receiptDto.CodePrefix;
            receipt.CashierId = receiptDto.CashierId;
            receipt.StoreId = receiptDto.StoreId;
            receipt.TotalPrice = receiptDto.TotalPrice;
            receipt.ReceiptItems = receiptDto.ReceiptItems.Select(x => x.ToEntity()).ToList();

            await _receiptRepository.UpdateReceiptAsync(receipt);

            return receipt.ToDto();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while updating receipt: id {receiptDto.Id}");
            throw;
        }
    }

    public async Task DeleteReceiptAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            await _receiptRepository.DeleteReceiptAsync(id, isSoftDeleting);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while deleting receipt: id {id}");
            throw;
        }
    }
}
