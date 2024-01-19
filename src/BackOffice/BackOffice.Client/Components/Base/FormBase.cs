using BackOffice.Client.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Components.Base;

public abstract class FormBase<TModel> : OrsComponentBase where TModel : class, new()
{
    [Parameter]
    public List<ToolbarCommand> ToolbarCommands { get; set; } = null!;

    [Inject] protected TabsService TabsService { get; set; } = null!;

    protected EditContext? EditContext { get; set;}

    protected abstract void DefineFormToolbarCommands();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        DefineFormToolbarCommands();
    }

    protected virtual void CloseClick()
    {
        TabsService.RemoveTab(TabsService.Tabs?.ActivePanel);
    }
}
