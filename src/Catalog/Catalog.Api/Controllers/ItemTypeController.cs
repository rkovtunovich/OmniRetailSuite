using Catalog.Application.DTOs.CatalogTDOs;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/v1/catalog")]
public class ItemTypeController: ControllerBase
{
    private readonly IItemTypeRepository _itemTypeRepository;

    public ItemTypeController(IItemTypeRepository itemTypeRepository)
    {
        _itemTypeRepository = itemTypeRepository;
    }

    // GET api/v1/[controller]/CatalogTypes
    [HttpGet]
    [Route("types")]
    [ProducesResponseType(typeof(List<ItemTypeDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ItemTypeDto>>> CatalogTypesAsync()
    {
        var types = await _itemTypeRepository.GetItemTypesAsync();

        return types.AsQueryable().Select(ItemTypeDto.Projection).ToList();
    }

    [HttpGet]
    [Route("types/{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ItemTypeDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ItemTypeDto>> TypeByIdAsync(int id)
    {
        if (id <= 0)
            return BadRequest();

        var type = await _itemTypeRepository.GetItemTypeByIdAsync(id);

        if (type is not null)
            return ItemTypeDto.FromItemType(type);

        return NotFound();
    }

    //POST api/v1/[controller]/types
    [Route("types")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateTypeAsync([FromBody] ItemTypeDto type)
    {
        var itemType = type.ToEntity();
        bool created = await _itemTypeRepository.CreateItemTypeAsync(itemType);

        if (!created)
            return BadRequest();

        return Created("", null);
    }

    //PUT api/v1/[controller]/types
    [Route("types")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> UpdateTypeAsync([FromBody] ItemTypeDto typeToUpdate)
    {
        var itemType = typeToUpdate.ToEntity();
        var updated = await _itemTypeRepository.UpdateItemTypeAsync(itemType);
        if (!updated)
            return NotFound();

        return Ok();
    }

    //DELETE api/v1/[controller]/types/id
    [Route("types/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteTypeAsync(int id, [FromQuery] bool useSoftDeleting = false)
    {
        var deleted = await _itemTypeRepository.DeleteItemTypeAsync(id, useSoftDeleting);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
