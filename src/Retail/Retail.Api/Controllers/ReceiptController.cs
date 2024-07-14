using System.Net.Mime;
using Contracts.Dtos.Retail;
using Infrastructure.Serialization.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Retail.Api.Controllers;

[ApiController]
[Route("api/v1/{resource}")]
public class ReceiptController(IReceiptService receiptService, IDataSerializer dataSerializer, ILogger<ReceiptController> logger): ControllerBase
{
    private readonly IReceiptService _receiptService = receiptService;
    private readonly ILogger<ReceiptController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(typeof(List<ReceiptDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReceiptsAsync()
    {
        try
        {
            var receipts = await _receiptService.GetReceiptsAsync();
            var serializedReceipts = dataSerializer.Serialize(receipts);

            return Content(serializedReceipts, MediaTypeNames.Application.Json);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting receipts");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [Route("{id:Guid}")]
    [ProducesResponseType(typeof(ReceiptDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReceiptAsync(Guid id)
    {
        try
        {
            var receipt = await _receiptService.GetReceiptAsync(id);

            if (receipt is null)         
                return NotFound();

            var serializedReceipt = dataSerializer.Serialize(receipt);

            return Content(serializedReceipt, MediaTypeNames.Application.Json);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting receipt");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReceiptDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReceiptAsync(ReceiptDto receipt)
    {
        try
        {
            var createdReceipt = await _receiptService.CreateReceiptAsync(receipt);

            return Created();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating receipt");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(ReceiptDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateReceiptAsync(ReceiptDto receipt)
    {
        try
        {
            await _receiptService.UpdateReceiptAsync(receipt);

            var serializedReceipt = dataSerializer.Serialize(receipt);

            return Content(serializedReceipt, MediaTypeNames.Application.Json);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating receipt");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteReceiptAsync(Guid id, bool useSoftDeleting)
    {
        try
        {
            await _receiptService.DeleteReceiptAsync(id, useSoftDeleting);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting receipt");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
