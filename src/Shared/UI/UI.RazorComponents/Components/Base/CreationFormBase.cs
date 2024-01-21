using Microsoft.AspNetCore.Components.Forms;

namespace UI.Razor.Components.Base;

public abstract class CreationFormBase<TModel> : FormBase<TModel> where TModel : class, new()
{
    [Parameter]
    public TModel Model { get; set; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        EditContext = new EditContext(Model);
    }
}
