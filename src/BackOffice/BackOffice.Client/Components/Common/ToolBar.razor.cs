namespace BackOffice.Client.Components.Common;

public partial class ToolBar
{
    [Parameter]
    public List<ToolbarCommand> Commands { get; set; } = null!;
}
