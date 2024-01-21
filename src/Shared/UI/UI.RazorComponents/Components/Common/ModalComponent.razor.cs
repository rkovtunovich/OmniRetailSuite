namespace UI.Razor.Components.Common;

public partial class ModalComponent
{
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    void Submit() => MudDialog.Close(DialogResult.Ok(true));

    void Cancel() => MudDialog.Cancel();
}
