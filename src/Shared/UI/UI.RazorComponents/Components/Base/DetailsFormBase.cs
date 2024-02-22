using Microsoft.AspNetCore.Components.Forms;

namespace UI.Razor.Components.Base;

public abstract class DetailsFormBase<TModel> : FormBase<TModel> where TModel : class, new()
{
    [Parameter]
    public Guid Id { get; set; }

    protected TModel Model = new();

    protected override void OnInitialized()
    {
        EditContext = new EditContext(Model);

        base.OnInitialized();

        DefineFormToolbarCommands();
    }

    protected override async Task OnInitializedAsync()
    {
        Model = await GetModel();
        EditContext = new EditContext(Model);
    }

    protected abstract Task<TModel> GetModel();
}
