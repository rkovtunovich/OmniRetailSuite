namespace BackOffice.Core.Models.Catalog;

public class ItemParent
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ItemParent? Parent { get; set; }
}
