namespace UI.Razor.Components.Base;

public class TabularPartBase<TItem> : OrsComponentBase
{
    [Parameter]
    public List<TItem> Items { get; set; } = [];

    [Parameter]
    public List<ToolbarCommand> Commands { get; set; } = [];
}
