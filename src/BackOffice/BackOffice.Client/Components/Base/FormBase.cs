using BackOffice.Client.Components.Common;
using BackOffice.Client.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Components.Base;

public abstract class FormBase<TModel> : OrsComponent where TModel : class, new()
{
    [Parameter]
    public List<ToolbarCommand> ToolbarCommands { get; set; } = null!;

    [Inject] protected TabsService TabsService { get; set; } = null!;

    [Parameter]
    public TModel Model { get; set; } = new();

    protected EditContext? EditContext { get; set;}

    protected abstract void DefineToolbarCommands();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        EditContext = new EditContext(Model);

        DefineToolbarCommands();
    }

    protected virtual void CloseClick()
    {
        TabsService.RemoveTab(TabsService.Tabs?.ActivePanel);
    }
}
