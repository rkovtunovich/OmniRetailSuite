﻿namespace BackOffice.Core.Models.Product;

public class ItemParent
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ItemParent? Parent { get; set; }

    public Guid? ParentId { get; set; }

    public HashSet<ItemParent>? Children { get; set; }
}
