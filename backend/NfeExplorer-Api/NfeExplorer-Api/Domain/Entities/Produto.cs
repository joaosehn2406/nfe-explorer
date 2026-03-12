namespace NfeExplorer_Api.Domain.Entities;

public class Produto
{
    public Guid Id { get; set; }
    public required string CodigoProduto { get; set; }
    public required string Descricao { get; set; }
    public required string NCM { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    
    public Guid IdNotaFiscal { get; set; }
    public NotaFiscal NotaFiscal { get; set; }
    
    
}