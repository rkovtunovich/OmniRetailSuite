using Contracts.Events.Item;
using Infrastructure.Messaging.Abstraction;

namespace Retail.Application.EventsHandlers.CatalogItem;

public class ItemDeletedEventHandler: IEventHandler<ItemDeletedEvent>
{
    private readonly IProductItemService _catalogItemService;
    private readonly ILogger<ItemDeletedEventHandler> _logger;

    public ItemDeletedEventHandler(IProductItemService catalogItemService, ILogger<ItemDeletedEventHandler> logger)
    {
        _catalogItemService = catalogItemService;
        _logger = logger;
    }

    public void Handle(ItemDeletedEvent @event)
    {
        _logger.LogInformation($"Handling event: {@event.GetType().Name}, id {@event.Id}");

        try
        {
            _catalogItemService.DeleteProductItemAsync(@event.Id, true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while handling event: {@event.GetType().Name}, id {@event.Id}");
            throw;
        }
    }
}
