namespace NfeExplorer_Api.Domain.Entities;

public class Recipient
{
    public Guid Id { get; set; }
    public required string LegalName { get; set; }
    public string? CNPJ { get; set; }
    public string? CPF { get; set; }
    public string? StateRegistration { get; set; }
    public required string Street { get; set; }
    public required string Number { get; set; }
    public required string District { get; set; }
    public required string City { get; set; }
    public required string UF { get; set; }
    public string? PersonName { get; set; }
    public required string ZipCode { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
