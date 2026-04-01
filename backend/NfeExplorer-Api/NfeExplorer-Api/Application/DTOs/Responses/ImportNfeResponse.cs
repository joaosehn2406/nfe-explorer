using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.DTOs.Responses;

public class ImportNfeResponse
{
    public Guid Id { get; set; }
    public string NumeroNota { get; set; }
    public string Emitente { get; set; }
    public decimal ValorTotal { get; set; }
    public TipoNota TipoNota { get; set; }
}