namespace BackOffice.Client.Model.UI;

public class TableColumn<TValue> where TValue : class
{
    public string Name { get; set; } = null!;

    public TValue Value { get; set; } = null!;
}
