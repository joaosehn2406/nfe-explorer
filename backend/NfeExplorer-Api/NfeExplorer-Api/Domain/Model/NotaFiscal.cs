using System.ComponentModel.DataAnnotations.Schema;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Domain.Model;

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

    public Guid IdEmitente { get; set; }
    public Guid IdDestinatario { get; set; }
    public Guid? IdTransportadora { get; set; }

    [ForeignKey("IdEmitente")]
    public Emitente Emitente { get; set; }

    [ForeignKey("IdDestinatario")]
    public Destinatario Destinatario { get; set; }

    [ForeignKey("IdTransportadora")]
    public Transportadora? Transportadora { get; set; }

    public ImpostosNfe? ImpostosNfe { get; set; }
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}