using Microsoft.AspNetCore.Mvc;
using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Application.Interfaces;
using NfeExplorer_Api.Domain.Interfaces;

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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _notaFiscalService.GetByIdAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("chave/{chave}")]
    public async Task<IActionResult> GetByChave(string chave)
    {
        var result = await _notaFiscalService.GetByChaveAsync(chave);
        return result is not null ? Ok(result) : NotFound();
    }
}