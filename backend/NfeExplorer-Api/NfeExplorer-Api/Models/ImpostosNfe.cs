using System.ComponentModel.DataAnnotations.Schema;

namespace NfeExplorer_Api.Models;

public class ImpostosNfe
{
    public Guid Id { get; set; }
    public required Guid IdNotaFiscal { get; set; }
    
    public decimal ValorProdutos { get; set; }
    
    public decimal BaseCalculoICMS { get; set; }
    public decimal ValorICMS { get; set; }
    
    public decimal ValorPIS { get; set; }
    
    public decimal ValorCOFINS { get; set; }
    
    public decimal ValorTotalTributos { get; set; }
    public decimal ValorNota { get; set; }
    public decimal AliquotaIcms { get; set; }
    
    [ForeignKey("IdNotaFiscal")]
    public NotaFiscal NotaFiscal { get; set; }
}