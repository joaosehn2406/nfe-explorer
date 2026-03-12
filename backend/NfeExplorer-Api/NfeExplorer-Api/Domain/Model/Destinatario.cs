namespace NfeExplorer_Api.Domain.Model;

public class Destinatario
{
    public Guid Id { get; set; }
    public required string RazaoSocial { get; set; }
    public string? CNPJ { get; set; }
    public string? CPF { get; set; }
    public string? InscricaoEstadual { get; set; }
    public required string Logradouro { get; set; }
    public required string Numero { get; set; }
    public required string Bairro { get; set; }
    public required string Municipio { get; set; }
    public required string UF { get; set; }
    public string? NomePessoa { get; set; }
    public required string CEP { get; set; }

    public ICollection<NotaFiscal> NotasFiscais { get; set; } = new List<NotaFiscal>();
}