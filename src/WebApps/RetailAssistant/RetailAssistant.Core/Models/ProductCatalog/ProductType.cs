﻿namespace RetailAssistant.Core.Models.ProductCatalog;

public class ProductType : EntityModelBase
{
    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? o)
    {
        var other = o as ProductType;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
