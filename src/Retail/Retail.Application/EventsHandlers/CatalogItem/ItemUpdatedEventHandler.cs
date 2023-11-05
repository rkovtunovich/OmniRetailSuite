using Contracts.Events.Item;
using Infrastructure.Messaging.Abstraction;

namespace Retail.Application.EventsHandlers.CatalogItem;

public class ItemUpdatedEventHandler : IEventHandler<ItemUpdatedEvent>
{
    private readonly ICatalogItemService _catalogItemService;
    private readonly ILogger<ItemUpdatedEventHandler> _logger;

    public ItemUpdatedEventHandler(ICatalogItemService catalogItemService, ILogger<ItemUpdatedEventHandler> logger)
    {
        _catalogItemService = catalogItemService;
        _logger = logger;
    }

    public void Handle(ItemUpdatedEvent @event)
    {
        _logger.LogInformation($"Handling event: {@event.GetType().Name}, id {@event.Id}");

        try
        {
            var catalogItemDto = new CatalogItemDto
            {
                Id = @event.Id,
                Name = @event.Name
            };

            _catalogItemService.UpdateCatalogItemAsync(catalogItemDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while handling event: {@event.GetType().Name}, id {@event.Id}");
            throw;
        }
    }
}
