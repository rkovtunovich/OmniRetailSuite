using Contracts.Dtos.Retail;
using Contracts.Events.Item;
using Infrastructure.Messaging.Abstraction;
using Retail.Application.EventsHandlers.CatalogItem;
using Retail.Application.Mapping.Configuration;

namespace Retail.Api.Configuration;

public static class ConfigureAppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ICashierService, CashierService>();
        services.AddScoped<IProductItemService, ProductItemService>();
        services.AddScoped<IReceiptService, ReceiptService>();
        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<IRetailService<AppClientSettingsDto>, RetailService<AppClientSettings, AppClientSettingsDto>>();

        services.AddScoped<IEventHandler<ItemUpdatedEvent>, ItemUpdatedEventHandler>();
        services.AddScoped<IEventHandler<ItemCreatedEvent>, ItemCreatedEventHandler>();
        services.AddScoped<IEventHandler<ItemDeletedEvent>, ItemDeletedEventHandler>();

        services.AddMapping();

        return services;
    }
}
