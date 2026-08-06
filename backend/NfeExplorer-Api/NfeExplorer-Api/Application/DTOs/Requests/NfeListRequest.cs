using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.DTOs.Requests;

public class NfeListRequest
{
    public string? Search { get; set; }
    public TipoNota? Tipo { get; set; }
    public string? Emitente { get; set; }
    public DateTime? DataDe { get; set; }
    public DateTime? DataAte { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
