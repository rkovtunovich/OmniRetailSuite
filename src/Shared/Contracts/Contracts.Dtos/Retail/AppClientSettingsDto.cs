namespace Contracts.Dtos.Retail;

public record class AppClientSettingsDto : EntityDtoBase
{
    public Guid StoreId { get; set; }
}
