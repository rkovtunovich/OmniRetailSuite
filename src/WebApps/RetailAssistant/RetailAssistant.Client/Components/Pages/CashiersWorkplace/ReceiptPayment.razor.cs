﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace RetailAssistant.Client.Components.Pages.CashiersWorkplace;

public partial class ReceiptPayment
{
    [Inject] private IStringLocalizer<ReceiptPayment> _localizer { get; set; } = default!;

    [Parameter]
    public decimal TotalPrice { get; set; }

    [Parameter]
    public EventCallback<decimal> OnPaymentCompleted { get; set; }

    [Parameter]
    public EventCallback OnPaymentCancelled { get; set; }

    private decimal _paymentAmount;

    private decimal _changeAmount;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _paymentAmount = TotalPrice;
    }

    private void CancelClick(MouseEventArgs e)
    {
        if (OnPaymentCancelled.HasDelegate)
            OnPaymentCancelled.InvokeAsync();
    }
    private void PayClick(MouseEventArgs e)
    {
        if (_paymentAmount < TotalPrice)
            return;

        if (OnPaymentCompleted.HasDelegate)
            OnPaymentCompleted.InvokeAsync(_paymentAmount);
    }

    private void PaymentAmountChanged(string text)
    {
        if (TotalPrice > _paymentAmount)
            _changeAmount = 0;
        else
            _changeAmount = _paymentAmount - TotalPrice;
    }
}
