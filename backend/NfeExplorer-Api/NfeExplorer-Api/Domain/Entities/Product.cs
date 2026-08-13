namespace NfeExplorer_Api.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string ProductCode { get; set; }
    public required string Description { get; set; }
    public required string NCM { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitAmount { get; set; }
    public decimal TotalAmount { get; set; }

    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = default!;
}
