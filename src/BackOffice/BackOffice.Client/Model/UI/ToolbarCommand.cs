using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Model.UI;

public class ToolbarCommand
{
    public string Name { get; set; } = null!;

    public string? Icon { get; set; }

    public EventCallback<MouseEventArgs> Callback { get; set; }

    public string? Tooltip { get; set; }
}
