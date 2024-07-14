using Microsoft.AspNetCore.Components.Web;

namespace UI.Razor.Models;

public class ToolbarCommand
{
    public string Name { get; set; } = null!;

    public string? Icon { get; set; }

    public string? Tooltip { get; set; }

    public string? CssClass { get; set; }

    public EventCallback<MouseEventArgs> Callback { get; set; }
}
