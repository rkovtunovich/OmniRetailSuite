﻿using Retail.Core.Entities;

namespace Retail.Core.Repositories;

public interface ICashierRepository
{
    Task<List<Cashier>> GetCashiersAsync();

    Task<Cashier?> GetCashierAsync(Guid cashierId);

    Task<Cashier> AddCashierAsync(Cashier cashier);

    Task UpdateCashierAsync(Cashier cashier);

    Task DeleteCashierAsync(Guid cashierId, bool isSoftDeleting);
}
