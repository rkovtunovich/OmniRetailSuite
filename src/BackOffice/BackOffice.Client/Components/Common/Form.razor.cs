namespace BackOffice.Client.Components.Common;

public partial class Form
{
    [Parameter]
    public List<ToolbarCommand> Commands { get; set; } = null!;

    [Parameter]
    public RenderFragment FormContent { get; set; } = null!;
}
