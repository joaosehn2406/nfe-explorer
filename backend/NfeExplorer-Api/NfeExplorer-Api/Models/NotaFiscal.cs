using System.ComponentModel.DataAnnotations.Schema;
using NfeExplorer_Api.Models.Enums;

namespace NfeExplorer_Api.Models;

public class NotaFiscal
{
    public Guid Id { get; set; }
    public required string ChaveAcesso { get; set; }
    public required DateTime DataEmissao { get; set; }
    public required DateTime DataImportacao { get; set; }
    public required string NaturezaOperacao { get; set; }
    public required string NumeroNota { get; set; }
    public required string Serie { get; set; }
    public required decimal ValorTotal { get; set; }
    public decimal ValorPago { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public TipoNota TipoNota { get; set; }

    public required Guid IdEmitente { get; set; }
    public required Guid IdDestinatario { get; set; }
    public Guid? IdTransportadora { get; set; }

    [ForeignKey("IdEmitente")]
    public required Emitente Emitente { get; set; }

    [ForeignKey("IdDestinatario")]
    public required Destinatario Destinatario { get; set; }

    [ForeignKey("IdTransportadora")]
    public Transportadora? Transportadora { get; set; }

    public ImpostosNfe? ImpostosNfe { get; set; }
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}