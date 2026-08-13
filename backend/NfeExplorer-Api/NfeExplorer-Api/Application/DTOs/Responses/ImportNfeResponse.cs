using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.DTOs.Responses;

public class ImportNfeResponse
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public InvoiceType InvoiceType { get; set; }
}
