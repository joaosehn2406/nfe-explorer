using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Domain.Entities;

public class Carrier
{
    public Guid Id { get; set; }
    public required string LegalName { get; set; }
    public string? CNPJ { get; set; }
    public string? CPF { get; set; }
    public string? StateRegistration { get; set; }
    public string? City { get; set; }
    public string? UF { get; set; }
    public FreightMode FreightMode { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
