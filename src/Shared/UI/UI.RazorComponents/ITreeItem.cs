namespace UI.Razor;

public interface ITreeItem
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public HashSet<ITreeItem> Children { get; set; }

    public string GetCode();
}
