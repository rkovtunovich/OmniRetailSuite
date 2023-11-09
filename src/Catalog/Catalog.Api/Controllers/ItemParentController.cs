using Catalog.Application.DTOs.CatalogTDOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/v1/catalog")]
public class ItemParentController: ControllerBase
{
    private readonly IItemParentRepository _itemParentRepository;
    private readonly ILogger<ItemParentController> _logger;

    public ItemParentController(IItemParentRepository itemParentRepository, ILogger<ItemParentController> logger)
    {
        _itemParentRepository = itemParentRepository;
        _logger = logger;
    }

    // GET api/v1/[controller]/CatalogParents
    [HttpGet]
    [Route("parents")]
    [ProducesResponseType(typeof(List<ItemParentDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ItemParentDto>>> CatalogParentsAsync()
    {
        var parents = await _itemParentRepository.GetItemParentsAsync();

        return parents.AsQueryable().Select(ItemParentDto.Projection).ToList();
    }

    [HttpGet]
    [Route("parents/{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ItemParentDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ItemParentDto>> ParentByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var parent = await _itemParentRepository.GetItemParentAsync(id);

        if (parent is null)
            return NotFound();

        return ItemParentDto.FromItemParent(parent);
    }

    //POST api/v1/[controller]/parents
    [Route("parents")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateParentAsync([FromBody] ItemParentDto parent)
    {
        var item = parent.ToEntity();
        
        var isCreated = await _itemParentRepository.CreateItemParentAsync(item);
        if (isCreated)
            return CreatedAtAction(nameof(ParentByIdAsync), new { id = item.Id }, null);

        return BadRequest();
    }

    //PUT api/v1/[controller]/parents
    [Route("parents")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> UpdateParentAsync([FromBody] ItemParentDto parentToUpdate)
    {
        var parent = parentToUpdate.ToEntity();

        var isUpdated = await _itemParentRepository.UpdateItemParentAsync(parent);
        if (!isUpdated)
            return NotFound();

        return Ok();
    }

    //DELETE api/v1/[controller]/parents/id
    [Route("parents/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteParentAsync(Guid id, [FromQuery] bool useSoftDeleting = false)
    {
        var isDeleted = await _itemParentRepository.DeleteItemParentAsync(id, useSoftDeleting);
        if (!isDeleted)
            return NotFound();

        return NoContent();
    }
}
