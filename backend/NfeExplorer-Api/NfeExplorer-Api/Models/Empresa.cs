namespace NfeExplorer_Api.Models;

public abstract class Empresa
{
    public Guid Id { get; set; }
    public required string RazaoSocial { get; set; }
    public required string Logradouro { get; set; }
    public required string Numero { get; set; }
    public required string Bairro { get; set; }
    public required string Municipio { get; set; }
    public required string UF { get; set; }
    public required string CEP { get; set; }
}