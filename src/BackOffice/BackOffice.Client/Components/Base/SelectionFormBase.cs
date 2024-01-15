namespace BackOffice.Client.Components.Base;

public class SelectionFormBase<TItem> where TItem : EntityModelBase
{
    [Parameter]
    public List<TItem> Items { get; set; } = [];
}
