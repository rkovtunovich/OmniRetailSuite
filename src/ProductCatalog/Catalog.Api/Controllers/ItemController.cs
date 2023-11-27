using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/v1/catalog")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly ILogger<ItemController> _logger;

    public ItemController(IItemService itemService, ILogger<ItemController> logger)
    {
        _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
        _logger = logger;
    }

    // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
    [HttpGet]
    [Route("items")]
    [ProducesResponseType(typeof(PaginatedItemsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IEnumerable<ItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        var items = await _itemService.GetItemsAsync(pageSize, pageIndex);

        return Ok(items);
    }

    [HttpGet]
    [Route("items/{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ItemDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ItemDto>> ItemByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var item = await _itemService.GetItemByIdAsync(id);

        if (item is null)
            return NotFound();

        return item;
    }

    // GET api/v1/[controller]/items/withname/samplename[?pageSize=3&pageIndex=10]
    [HttpGet]
    [Route("items/withname/{name:minlength(1)}")]
    [ProducesResponseType(typeof(ItemDto), (int)HttpStatusCode.OK)]
    public async Task<List<ItemDto>> ItemsWithNameAsync(string name, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        var items = await _itemService.GetItemsByNameAsync(name);

        return items;
    }

    // GET api/v1/[controller]/items/type/1/brand[?pageSize=3&pageIndex=10]
    [HttpGet]
    [Route("items/type/{catalogTypeId}/brand/{catalogBrandId:int?}")]
    [ProducesResponseType(typeof(PaginatedItemsDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginatedItemsDto>> ItemsByTypeIdAndBrandIdAsync(Guid? catalogTypeId, Guid? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        return await _itemService.GetItemsByCategoryAsync(catalogBrandId, catalogTypeId);
    }

    // GET api/v1/[controller]/items/type/all/brand[?pageSize=3&pageIndex=10]
    [HttpGet]
    [Route("items/type/all/brand/{catalogBrandId:int?}")]
    [ProducesResponseType(typeof(PaginatedItemsDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginatedItemsDto>> ItemsByBrandIdAsync(Guid? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        if (catalogBrandId is null)
            throw new ArgumentNullException(nameof(catalogBrandId));

        return await _itemService.GetItemsByCategoryAsync(catalogBrandId, null);
    }

    // GET api/v1/[controller]/items/parent/{id}[?pageSize=3&pageIndex=10]
    [HttpGet]
    [Route("items/parent/{catalogParentId}")]
    [ProducesResponseType(typeof(PaginatedItemsDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginatedItemsDto>> ItemsByParentIdAsync(Guid catalogParentId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        return await _itemService.GetItemsByParentAsync(catalogParentId);
    }

    //PUT api/v1/[controller]/items
    [Route("items")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> UpdateItemAsync([FromBody] ItemDto itemDto)
    {
        var updatedItem = await _itemService.UpdateItemAsync(itemDto);
        if (updatedItem is not null)
            return Ok();
        else
            return NotFound();
    }

    //POST api/v1/[controller]/items
    [Route("items")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateItemAsync([FromBody] ItemDto item)
    {
        var createdItem = await _itemService.CreateItemAsync(item);

        if (createdItem is null)
            return BadRequest();

        return Created();
    }

    //DELETE api/v1/[controller]/id
    [Route("items/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteItemAsync(Guid id, [FromQuery] bool useSoftDeleting = false)
    {
        var isDeleted = await _itemService.DeleteItemAsync(id, useSoftDeleting);
        if (!isDeleted)
            return NotFound();

        return NoContent();
    }
}
