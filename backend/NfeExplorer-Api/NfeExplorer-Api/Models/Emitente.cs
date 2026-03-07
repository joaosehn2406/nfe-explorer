namespace NfeExplorer_Api.Models;

public class Emitente : Empresa
{
    public required string CNPJ { get; set; }
    public string? NomeFantasia { get; set; }
    public string? InscricaoEstadual { get; set; }

    public ICollection<NotaFiscal> NotasFicais { get; set; } = new List<NotaFiscal>();
}