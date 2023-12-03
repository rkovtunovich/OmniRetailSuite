namespace BackOffice.Client.Model.UI;

public class TabView
{
    public string Name { get; set; } = string.Empty;

    public RenderFragment Content { get; set; } = null!;

    public Guid Id { get; set; } = Guid.Empty;
}
