using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Core.Repositories;

public interface ICashierRepository
{
    Task<List<Cashier>> GetCashiersAsync();

    Task<Cashier?> GetCashierAsync(int cashierId);

    Task<Cashier> AddCashierAsync(Cashier cashier);

    Task UpdateCashierAsync(Cashier cashier);

    Task DeleteCashierAsync(int cashierId, bool useSoftDeleting);
}
