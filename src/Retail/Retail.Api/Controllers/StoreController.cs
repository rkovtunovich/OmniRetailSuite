using Contracts.Dtos.Retail;
using Microsoft.AspNetCore.Mvc;
using Retail.Application.Services.Abstraction;

namespace Retail.Api.Controllers;

public class StoreController: ControllerBase
{
    IStoreService _storeService;
    ILogger<StoreController> _logger;

    public StoreController(IStoreService storeService, ILogger<StoreController> logger)
    {
        _storeService = storeService;
        _logger = logger;
    }

    [HttpGet]
    [Route("stores")]
    [ProducesResponseType(typeof(List<StoreDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<StoreDto>>> GetStoresAsync()
    {
        try
        {
            var stores = await _storeService.GetStoresAsync();

            return Ok(stores);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting stores");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [Route("stores/{id:Guid}")]
    [ProducesResponseType(typeof(StoreDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StoreDto>> GetStoreAsync(Guid id)
    {
        try
        {
            var store = await _storeService.GetStoreAsync(id);

            if (store is null)
                return NotFound();

            return Ok(store);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting store");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Route("stores")]
    [ProducesResponseType(typeof(StoreDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StoreDto>> CreateStoreAsync(StoreDto storeDto)
    {
        try
        {
            var createdStore = await _storeService.CreateStoreAsync(storeDto);

            return Created();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating store");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPut]
    [Route("stores")]
    [ProducesResponseType(typeof(StoreDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StoreDto>> UpdateStoreAsync(StoreDto storeDto)
    {
        try
        {
            var updatedStore = await _storeService.UpdateStoreAsync(storeDto);

            return Ok(updatedStore);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating store");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpDelete]
    [Route("stores/{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteStoreAsync(Guid id)
    {
        try
        {
            await _storeService.DeleteStoreAsync(id, true);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting store");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
