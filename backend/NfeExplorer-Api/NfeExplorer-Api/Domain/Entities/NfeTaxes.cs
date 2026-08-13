using System.ComponentModel.DataAnnotations.Schema;

namespace NfeExplorer_Api.Domain.Entities;

public class NfeTaxes
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }

    public decimal ProductAmount { get; set; }
    public decimal IcmsTaxBase { get; set; }
    public decimal IcmsAmount { get; set; }
    public decimal PisAmount { get; set; }
    public decimal CofinsAmount { get; set; }
    public decimal TotalTaxesAmount { get; set; }
    public decimal InvoiceAmount { get; set; }
    public decimal IcmsRate { get; set; }

    [ForeignKey(nameof(InvoiceId))]
    public Invoice Invoice { get; set; } = default!;
}
