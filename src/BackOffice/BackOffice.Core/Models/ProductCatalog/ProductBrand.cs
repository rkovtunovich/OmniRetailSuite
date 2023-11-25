﻿namespace BackOffice.Core.Models.ProductCatalog;

public class ProductBrand
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; } 

    public override string? ToString()
    {
        return Name;
    }

    public override bool Equals(object? o)
    {
        var other = o as ProductBrand;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public string GetCode()
    {
        return $"{CodePrefix}{CodeNumber:0000}";
    }
}
