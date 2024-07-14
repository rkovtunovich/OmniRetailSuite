namespace UI.Razor.Models;

public class HierarchySelectModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public List<HierarchySelectModel>? Children { get; set; }
}
