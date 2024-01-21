namespace UI.Razor.Components.Common;

public partial class ToolBar
{
    [Parameter]
    public List<ToolbarCommand> Commands { get; set; } = null!;
}
