using Microsoft.AspNetCore.Mvc;
using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Application.Interfaces;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.Controllers;

[ApiController]
[Route("api/nfe_explorer")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportNFe([FromForm] ParseNfeRequest request)
    {
        var result = await _invoiceService.AddAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] NfeListRequest filter)
    {
        var result = await _invoiceService.GetAllAsync(filter);
        return Ok(result);
    }

    [HttpGet("issuers")]
    public async Task<IActionResult> GetIssuers()
    {
        var result = await _invoiceService.GetIssuersAsync();
        return Ok(result);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await _invoiceService.GetDashboardAsync();
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] ImportStatus? status)
    {
        var result = await _invoiceService.GetHistoryAsync(status);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _invoiceService.GetByIdAsync(id);
        return result is not null ? Ok(result) : NotFound(new { code = 404, message = "NF-e not found." });
    }

    [HttpGet("access-key/{accessKey}")]
    public async Task<IActionResult> GetByAccessKey(string accessKey)
    {
        var result = await _invoiceService.GetByAccessKeyAsync(accessKey);
        return result is not null ? Ok(result) : NotFound(new { code = 404, message = "NF-e not found." });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var removed = await _invoiceService.DeleteAsync(id);
        return removed ? NoContent() : NotFound(new { code = 404, message = "NF-e not found." });
    }
}
