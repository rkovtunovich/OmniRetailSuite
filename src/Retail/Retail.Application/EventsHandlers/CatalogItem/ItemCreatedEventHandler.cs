using Infrastructure.Messaging.Abstraction;
using Contracts.Events.Item;

namespace Retail.Application.EventsHandlers.CatalogItem;

public class ItemCreatedEventHandler: IEventHandler<ItemCreatedEvent>
{
    private readonly IProductItemService _catalogItemService;
    private readonly ILogger<ItemCreatedEventHandler> _logger;

    public ItemCreatedEventHandler(IProductItemService catalogItemService, ILogger<ItemCreatedEventHandler> logger)
    {
        _catalogItemService = catalogItemService;
        _logger = logger;
    }

    public void Handle(ItemCreatedEvent @event)
    {
        _logger.LogInformation($"Handling event: {@event.GetType().Name}, id {@event.Id}");

        try
        {
            var catalogItemDto = new ProductItemDto
            {
                Id = @event.Id,
                Name = @event.Name
            };

            _catalogItemService.CreateProductItemAsync(catalogItemDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while handling event: {@event.GetType().Name}, id {@event.Id}");
            throw;
        }
    }
}
