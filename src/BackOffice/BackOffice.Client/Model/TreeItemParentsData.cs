using BackOffice.Core.Models.Catalog;

namespace BackOffice.Client.Model;

public class TreeItemParentsData
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public HashSet<TreeItemParentsData> Children { get; init; } = new();

    public static TreeItemParentsData Create(ItemParent itemParent)
    {
        var treeItemParentsData = new TreeItemParentsData()
        {
            Id = itemParent.Id,
            Name = itemParent.Name,
        };

        return treeItemParentsData;
    }
}
