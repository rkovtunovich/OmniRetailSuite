namespace UI.Razor.Components.Base;

public class ContextMenuItem
{
    public string Text { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public EventCallback OnClick { get; set; }
}
