namespace NfeExplorer_Api.Models;

public class Produto
{
    public Guid Id { get; set; }
    public required string CodigoProduto { get; set; }
    public required string Descricao { get; set; }
    public required string NCM { get; set; }
    public required string CFOP { get; set; }
    public required string Unidade { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    
    public required Guid IdNotaFiscal { get; set; }
    public required NotaFiscal NotaFiscal { get; set; }
    
    
}