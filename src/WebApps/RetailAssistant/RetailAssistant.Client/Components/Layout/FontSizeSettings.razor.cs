using Blazored.LocalStorage;
using Microsoft.JSInterop;

namespace RetailAssistant.Client.Components.Layout;

public partial class FontSizeSettings
{
    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    [Inject] private ILocalStorageService _localStorageService { get; set; } = default!;

    [Inject] private IStringLocalizer<FontSizeSettings> _localizer { get; set; } = default!;

    private double _fontSize = 1;

    protected override async Task OnInitializedAsync()
    {
        var fontSizeString = await _localStorageService.GetItemAsync<string>("fontSize");
        if (fontSizeString is not null)
            _fontSize = double.Parse(fontSizeString.Replace("em", "").Replace(".", ","));
    }

    private async Task ChangeFontSize(double size)
    {
        _fontSize = size;
        var sizeString = $"{_fontSize.ToString().Replace(",", ".")}em";
        await _jsRuntime.InvokeVoidAsync("changeFontSize", sizeString);
    }
}
