using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Entities;

namespace NfeExplorer_Api.Application.Mappings;

public static class NfeListMapper
{
    public static NfeListItemResponse ToListItem(Invoice invoice)
    {
        return new NfeListItemResponse
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            Series = invoice.Series,
            AccessKey = invoice.AccessKey,
            InvoiceType = invoice.InvoiceType,
            IssuedAt = invoice.IssuedAt,
            TotalAmount = invoice.TotalAmount,
            IssuerName = invoice.Issuer.TradeName ?? invoice.Issuer.LegalName,
            IssuerCnpj = invoice.Issuer.CNPJ,
            RecipientName = invoice.Recipient.LegalName
        };
    }
}
