namespace BackOffice.Client.Components.Base;

public class TabularPartBase<TItem> : OrsComponentBase where TItem : EntityModelBase
{
    [Parameter]
    public List<TItem> Items { get; set; } = [];

    [Parameter]
    public List<ToolbarCommand> Commands { get; set; } = [];
}
