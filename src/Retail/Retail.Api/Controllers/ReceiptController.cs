using Contracts.Dtos.Retail;
using Microsoft.AspNetCore.Mvc;
using Retail.Application.Services.Abstraction;
using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ReceiptController(IReceiptService receiptService, ILogger<ReceiptController> logger): ControllerBase
{
    private readonly IReceiptService _receiptService = receiptService;
    private readonly ILogger<ReceiptController> _logger = logger;

    [HttpGet]
    [Route("receipts")]
    [ProducesResponseType(typeof(List<ReceiptDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ReceiptDto>>> GetReceiptsAsync()
    {
        try
        {
            var receipts = await _receiptService.GetReceiptsAsync();

            return Ok(receipts);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting receipts");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [Route("receipts/{id:Guid}")]
    [ProducesResponseType(typeof(ReceiptDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReceiptDto>> GetReceiptAsync(Guid id)
    {
        try
        {
            var receipt = await _receiptService.GetReceiptAsync(id);

            if (receipt is null)         
                return NotFound();
            
            return Ok(receipt);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting receipt");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Route("receipts")]
    [ProducesResponseType(typeof(ReceiptDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReceiptDto>> CreateReceiptAsync(ReceiptDto receipt)
    {
        try
        {
            var createdReceipt = await _receiptService.CreateReceiptAsync(receipt);

            return CreatedAtAction(nameof(GetReceiptAsync), new { id = createdReceipt.Id }, createdReceipt);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating receipt");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPut]
    [Route("receipts")]
    [ProducesResponseType(typeof(ReceiptDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReceiptDto>> UpdateReceiptAsync(ReceiptDto receipt)
    {
        try
        {
            await _receiptService.UpdateReceiptAsync(receipt);

            return Ok(receipt);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating receipt");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpDelete]
    [Route("receipts/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Receipt>> DeleteReceiptAsync(Guid id, bool useSoftDeleting)
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
