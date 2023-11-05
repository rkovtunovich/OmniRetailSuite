namespace Catalog.Core.Exceptions;

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(int itemId) : base($"No item found with id {itemId}")
    {
    }
}
