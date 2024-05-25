using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace UI.Razor.Components.Common;

public partial class ContextMenu : OrsComponentBase
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    [Parameter]
    public List<ContextMenuItem> Items { get; set; } = null!;

    [Parameter]
    public EventCallback<bool> OnOpenChanged { get; set; }

    private string _contextMenuId = "ors-context-menu";
    private MudMenu _contextMenu = null!;

    public async Task ShowContextMenu(MouseEventArgs eventArg)
    {
        var contextMenu = await JSRuntime.InvokeAsync<IJSObjectReference>("document.getElementById", _contextMenuId);
        await contextMenu.InvokeVoidAsync("style.setProperty", "left", $"{eventArg.ClientX}px");
        await contextMenu.InvokeVoidAsync("style.setProperty", "top", $"{eventArg.ClientY}px");
        await contextMenu.InvokeVoidAsync("style.setProperty", "display", "block");

        await _contextMenu.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            { "Disabled", false }
        }));

        _contextMenu.OpenMenu(eventArg);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if(!OnOpenChanged.HasDelegate)
            OnOpenChanged = EventCallback.Factory.Create<bool>(this, OnContextMenuOpenChanged);
    }

    private async Task OnContextMenuOpenChanged(bool state)
    {
        if (_contextMenu.IsOpen)
            return;

        await _contextMenu.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            { "Disabled", !_contextMenu.Disabled }
        }));
    }
}
