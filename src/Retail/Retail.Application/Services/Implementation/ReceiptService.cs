using Retail.Core.Entities.ReceiptAggregate;
using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class ReceiptService(IReceiptRepository receiptRepository, IMapper mapper, ILogger<ReceiptService> logger) : IReceiptService
{
    public async Task<List<ReceiptDto>> GetReceiptsAsync()
    {
        try
        {
            var receipts = await receiptRepository.GetReceiptsAsync();
            return mapper.Map<List<ReceiptDto>>(receipts);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while getting receipts");
            throw;
        }
    }

    public async Task<ReceiptDto?> GetReceiptAsync(Guid id)
    {
        try
        {
            var receipt = await receiptRepository.GetReceiptAsync(id);

            return mapper.Map<ReceiptDto>(receipt);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while getting receipt: id {id}");
            throw;
        }
    }

    public async Task<ReceiptDto> CreateReceiptAsync(ReceiptDto receiptDto)
    {
        var receipt = mapper.Map<Receipt>(receiptDto);
        await receiptRepository.AddReceiptAsync(receipt);
        return mapper.Map<ReceiptDto>(receipt);
    }

    public async Task<ReceiptDto> UpdateReceiptAsync(ReceiptDto receiptDto)
    {
        try
        {
            var receipt = await receiptRepository.GetReceiptAsync(receiptDto.Id) ?? throw new Exception($"Receipt with id {receiptDto.Id} not found");
            receipt.Date = receiptDto.Date;
            receipt.CodeNumber = receiptDto.CodeNumber;
            receipt.CodePrefix = receiptDto.CodePrefix;
            receipt.CashierId = receiptDto.Cashier.Id;
            receipt.StoreId = receiptDto.Store.Id;
            receipt.TotalPrice = receiptDto.TotalPrice;
            receipt.ReceiptItems = mapper.Map<List<ReceiptItem>>(receiptDto.ReceiptItems);

            await receiptRepository.UpdateReceiptAsync(receipt);

            return mapper.Map<ReceiptDto>(receipt);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while updating receipt: id {receiptDto.Id}");
            throw;
        }
    }

    public async Task DeleteReceiptAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            await receiptRepository.DeleteReceiptAsync(id, isSoftDeleting);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while deleting receipt: id {id}");
            throw;
        }
    }
}
