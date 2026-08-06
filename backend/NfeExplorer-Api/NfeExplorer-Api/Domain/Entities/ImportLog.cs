using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Domain.Entities;

public class ImportLog
{
    public Guid Id { get; set; }
    public required DateTime DataHora { get; set; }
    public required StatusImportacao Status { get; set; }
    public required string NomeArquivo { get; set; }
    public string? NumeroNota { get; set; }
    public string? Emitente { get; set; }
    public decimal? Valor { get; set; }
    public string? Mensagem { get; set; }
}
