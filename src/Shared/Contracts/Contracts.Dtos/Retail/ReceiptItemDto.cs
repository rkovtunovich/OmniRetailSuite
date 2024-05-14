﻿namespace Contracts.Dtos.Retail;

public record ReceiptItemDto : EntityDtoBase
{
    public int LineNumber { get; init; }

    public Guid ReceiptId { get; init; }

    public Guid ProductItemId { get; init; }

    public double Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal TotalPrice { get; init; }
}
