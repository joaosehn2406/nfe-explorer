using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.DTOs.Responses;

public class NfeListItemResponse
{
    public Guid Id { get; set; }
    public string NumeroNota { get; set; }
    public string Serie { get; set; }
    public string ChaveAcesso { get; set; }
    public TipoNota TipoNota { get; set; }
    public DateTime DataEmissao { get; set; }
    public decimal ValorTotal { get; set; }
    public string EmitenteNome { get; set; }
    public string? EmitenteCnpj { get; set; }
    public string DestinatarioNome { get; set; }
}
