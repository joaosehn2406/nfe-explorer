using System.ComponentModel.DataAnnotations.Schema;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Domain.Entities;

public class Invoice
{
    public Guid Id { get; set; }
    public required string AccessKey { get; set; }
    public required DateTime IssuedAt { get; set; }
    public required DateTime ImportedAt { get; set; }
    public required string OperationNature { get; set; }
    public required string InvoiceNumber { get; set; }
    public required string Series { get; set; }
    public required decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public InvoiceType InvoiceType { get; set; }

    public Guid IssuerId { get; set; }
    public Guid RecipientId { get; set; }
    public Guid? CarrierId { get; set; }

    [ForeignKey(nameof(IssuerId))]
    public Issuer Issuer { get; set; } = default!;

    [ForeignKey(nameof(RecipientId))]
    public Recipient Recipient { get; set; } = default!;

    [ForeignKey(nameof(CarrierId))]
    public Carrier? Carrier { get; set; }

    public NfeTaxes? Taxes { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
