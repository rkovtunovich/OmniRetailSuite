using Retail.Core.Entities;

namespace Retail.Application.Mapping;

public static class StoreMapping
{
    public static StoreDto ToDto(this Store store)
    {
        return new StoreDto
        {
            Id = store.Id,
            Name = store.Name,
            Address = store.Address,
            CodeNumber = store.CodeNumber,
            CodePrefix = store.CodePrefix,
            Cashiers = store.Cashiers.Select(c => c.ToDto()).ToList(),
        };
    }

    public static Store ToEntity(this StoreDto storeDto)
    {
           return new Store
           {
                Id = storeDto.Id,
                Name = storeDto.Name,
                Address = storeDto.Address,
                CodeNumber = storeDto.CodeNumber,
                CodePrefix = storeDto.CodePrefix,
                Cashiers = storeDto.Cashiers.Select(c => c.ToEntity()).ToList(),
            };
    }
}
