using System.ComponentModel.DataAnnotations;
using Catalog.Core.Entities;

namespace Catalog.Core.Entities.CatalogAggregate;

public class ItemParent : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public int? ParentId { get; set; }

    public ItemParent? Parent { get; set; }
}
