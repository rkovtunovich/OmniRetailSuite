using BackOffice.Core.Models.Retail;
using Contracts.Dtos.Retail;

namespace BackOffice.Application.Mapping.Retail;

public static class StoreMapping
{
    public static Store ToModel(this StoreDto storeDto)
    {
        return new Store
        {
            Id = storeDto.Id,
            Name = storeDto.Name,
            CodeNumber = storeDto.CodeNumber,
            CodePrefix = storeDto?.CodePrefix,
            Cashiers = storeDto?.Cashiers?.Select(x => x.ToModel()).ToList() ?? []
        };
    }

    public static StoreDto ToDto(this Store store)
    {
        return new StoreDto
        {
            Id = store.Id,
            Name = store.Name,
            CodeNumber = store.CodeNumber,
            CodePrefix = store?.CodePrefix,
            Cashiers = store?.Cashiers?.Select(x => x.ToDto()).ToList() ?? []
        };
    }
}
