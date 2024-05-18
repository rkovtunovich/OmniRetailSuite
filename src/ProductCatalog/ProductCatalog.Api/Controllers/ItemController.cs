using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/v1/{resource}")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly ILogger<ItemController> _logger;

    public ItemController(IItemService itemService, ILogger<ItemController> logger)
    {
        _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
        _logger = logger;
    }

    // GET api/v1/{resource}[?pageSize=3&pageIndex=10]
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IEnumerable<ProductItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        var items = await _itemService.GetItemsAsync(pageSize, pageIndex);

        return Ok(items);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProductItemDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductItemDto>> ItemByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var item = await _itemService.GetItemByIdAsync(id);

        if (item is null)
            return NotFound();

        return item;
    }

    // GET api/v1/{resource}/withname/samplename[?pageSize=3&pageIndex=10]
    [HttpGet]
    [Route("withname/{name:minlength(1)}")]
    [ProducesResponseType(typeof(ProductItemDto), (int)HttpStatusCode.OK)]
    public async Task<List<ProductItemDto>> ItemsWithNameAsync(string name, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        var items = await _itemService.GetItemsByNameAsync(name);

        return items;
    }

    // GET api/v1/{resource}/type/1/brand[?pageSize=3&pageIndex=10]
    [HttpGet]
    [Route("type/{catalogTypeId}/brand/{catalogBrandId:int?}")]
    [ProducesResponseType(typeof(List<ProductItemDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ProductItemDto>>> ItemsByTypeIdAndBrandIdAsync(Guid? catalogTypeId, Guid? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        return await _itemService.GetItemsByCategoryAsync(catalogBrandId, catalogTypeId);
    }

    // GET api/v1/{resource}/type/all/brand[?pageSize=3&pageIndex=10]
    [HttpGet]
    [Route("type/all/brand/{catalogBrandId:int?}")]
    [ProducesResponseType(typeof(List<ProductItemDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ProductItemDto>>> ItemsByBrandIdAsync(Guid? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        if (catalogBrandId is null)
            throw new ArgumentNullException(nameof(catalogBrandId));

        return await _itemService.GetItemsByCategoryAsync(catalogBrandId, null);
    }

    // GET api/v1/{resource}/parent/{id}[?pageSize=3&pageIndex=10]
    [HttpGet]
    [Route("parent/{catalogParentId}")]
    [ProducesResponseType(typeof(List<ProductItemDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ProductItemDto>>> ItemsByParentIdAsync(Guid catalogParentId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        return await _itemService.GetItemsByParentAsync(catalogParentId);
    }

    //PUT api/v1/{resource}
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> UpdateItemAsync([FromBody] ProductItemDto itemDto)
    {
        var updatedItem = await _itemService.UpdateItemAsync(itemDto);
        if (updatedItem is not null)
            return Ok();
        else
            return NotFound();
    }

    //POST api/v1/{resource}
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateItemAsync([FromBody] ProductItemDto item)
    {
        var createdItem = await _itemService.CreateItemAsync(item);

        if (createdItem is null)
            return BadRequest();

        return Created();
    }

    //DELETE api/v1/{resource}/id
    [Route("/{id}")]
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
