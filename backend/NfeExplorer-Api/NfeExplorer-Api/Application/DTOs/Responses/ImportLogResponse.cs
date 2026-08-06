using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.DTOs.Responses;

public class ImportLogResponse
{
    public Guid Id { get; set; }
    public DateTime DataHora { get; set; }
    public StatusImportacao Status { get; set; }
    public string NomeArquivo { get; set; }
    public string? NumeroNota { get; set; }
    public string? Emitente { get; set; }
    public decimal? Valor { get; set; }
    public string? Mensagem { get; set; }
}
