namespace NfeExplorer_Api.Models;

public class NotaFiscal
{
    public Guid Id { get; set; }
    public required string ChaveAcesso { get; set; }
    public required DateTime DataEmissao { get; set; }
    public required DateTime DataImportacao { get; set; }
    public required string NaturezaOperacao { get; set; }
    public required decimal ValorTotal { get; set; }
    public required Guid IdEmitente { get; set; }
    public required Guid IdDestinatario { get; set; }
    public required Guid IdTipoNota { get; set; }
    public Guid? IdTransportadora { get; set; }
}