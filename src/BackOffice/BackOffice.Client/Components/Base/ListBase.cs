using BackOffice.Client.Components.Common;
using BackOffice.Client.Services;

namespace BackOffice.Client.Components.Base;

public abstract class ListBase<TItem> : OrsComponent where TItem : class
{
    [Parameter]
    public List<ToolbarCommand> ToolbarCommands { get; set; } = null!;

    [Inject] protected TabsService TabsService { get; set; } = null!;

    protected List<TItem> Items { get; set; } = [];

    protected string? SearchString;

    protected MudDataGrid<TItem> DataGrid { get; set; } = null!;

    protected abstract void DefineToolbarCommands();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        DefineToolbarCommands();
    }

    protected virtual void CloseClick()
    {
        TabsService.RemoveTab(TabsService.Tabs?.ActivePanel);
    }
}
