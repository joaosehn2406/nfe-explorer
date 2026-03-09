using NfeExplorer_Api.Models.Enums;

namespace NfeExplorer_Api.Models;

public class Transportadora
{
    public Guid Id { get; set; }
    public required string RazaoSocial { get; set; }
    public string? CNPJ { get; set; }
    public string? CPF { get; set; }
    public string? InscricaoEstadual { get; set; }
    public string? Municipio { get; set; }
    public string? UF { get; set; }
    public ModalidadeFrete ModalidadeFrete { get; set; }
    
    public ICollection<NotaFiscal> NotasFiscais { get; set; } = new List<NotaFiscal>();
}