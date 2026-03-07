namespace NfeExplorer_Api.Models;

public class Destinatario
{
    public string? CNPJ { get; set; }    
    public string? CPF { get; set; }     
    
    public ICollection<NotaFiscal> NotasFiscais { get; set; } = new List<NotaFiscal>();
}