using BackOffice.Application.Helpers;
using BackOffice.Application.Mapping.Retail;
using BackOffice.Application.Services.Abstraction.Retail;
using BackOffice.Core.Models.Retail;
using Contracts.Dtos.Retail;

namespace BackOffice.Application.Services.Implementation.Retail;

public class CashierService: IRetailService<Cashier>
{
    private readonly IHttpService<RetailResource> _httpService;
    private readonly ILogger<CashierService> _logger;

    public event Func<Cashier, Task>? OnChanged;

    public CashierService(IHttpService<RetailResource> httpService, ILogger<CashierService> logger)
    {
        _httpService = httpService;
        _logger = logger;
    }

    public async Task<List<Cashier>> GetAllAsync()
    {
        var uri = RetailUrlHelper.GetAll<Cashier>();
        var cashierDtos = await _httpService.GetAsync<List<CashierDto>>(uri);

        var cashiers = cashierDtos?.Select(x => x.ToModel()).ToList();

        return cashiers ?? [];
    }

    public async Task<Cashier?> GetByIdAsync(Guid cashierId)
    {
        var uri = RetailUrlHelper.Get<Cashier>(cashierId);
        var cashierDto = await _httpService.GetAsync<CashierDto>(uri);

        return cashierDto?.ToModel();
    }

    public async Task<Cashier> CreateAsync(Cashier cashier)
    {
        var uri = RetailUrlHelper.Add<Cashier>();
        await _httpService.PostAsync(uri, cashier.ToDto());

        OnChanged?.Invoke(cashier);

        return cashier;
    }

    public async Task<bool> UpdateAsync(Cashier cashier)
    {
        var uri = RetailUrlHelper.Update<Cashier>();
        await _httpService.PutAsync(uri, cashier.ToDto());

        OnChanged?.Invoke(cashier);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid cashierId, bool isSoftDeleting)
    {
        var uri = RetailUrlHelper.Delete<Cashier>(cashierId, isSoftDeleting);
        await _httpService.DeleteAsync(uri);

        OnChanged?.Invoke(new Cashier { Id = cashierId });

        return true;
    }
}
