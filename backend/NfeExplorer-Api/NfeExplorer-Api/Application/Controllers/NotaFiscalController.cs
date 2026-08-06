using Microsoft.AspNetCore.Mvc;
using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Application.Interfaces;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.Controllers;

[ApiController]
[Route("api/nfe_explorer")]
public class NotaFiscalController : ControllerBase
{
    private readonly INotaFiscalService _notaFiscalService;

    public NotaFiscalController(INotaFiscalService notaFiscalService)
    {
        _notaFiscalService = notaFiscalService;
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportNFe([FromForm] ParseNfeRequest request)
    {
        var result = await _notaFiscalService.AddAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] NfeListRequest filter)
    {
        var result = await _notaFiscalService.GetAllAsync(filter);
        return Ok(result);
    }

    [HttpGet("emitentes")]
    public async Task<IActionResult> GetEmitentes()
    {
        var result = await _notaFiscalService.GetEmitentesAsync();
        return Ok(result);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await _notaFiscalService.GetDashboardAsync();
        return Ok(result);
    }

    [HttpGet("historico")]
    public async Task<IActionResult> GetHistorico([FromQuery] StatusImportacao? status)
    {
        var result = await _notaFiscalService.GetHistoricoAsync(status);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _notaFiscalService.GetByIdAsync(id);
        return result is not null ? Ok(result) : NotFound(new { code = 404, message = "NF-e não encontrada." });
    }

    [HttpGet("chave/{chave}")]
    public async Task<IActionResult> GetByChave(string chave)
    {
        var result = await _notaFiscalService.GetByChaveAsync(chave);
        return result is not null ? Ok(result) : NotFound(new { code = 404, message = "NF-e não encontrada." });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var removido = await _notaFiscalService.DeleteAsync(id);
        return removido ? NoContent() : NotFound(new { code = 404, message = "NF-e não encontrada." });
    }
}
