using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.DTOs.Responses;

public class NfeListItemResponse
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string Series { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public InvoiceType InvoiceType { get; set; }
    public DateTime IssuedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public string IssuerName { get; set; } = string.Empty;
    public string? IssuerCnpj { get; set; }
    public string RecipientName { get; set; } = string.Empty;
}
