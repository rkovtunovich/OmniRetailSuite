namespace BackOffice.Core.Models.Retail;

public class Cashier
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; } = string.Empty;

    public string GetCode()
    {
        return $"{CodePrefix}{CodeNumber:0000}";
    }
}
