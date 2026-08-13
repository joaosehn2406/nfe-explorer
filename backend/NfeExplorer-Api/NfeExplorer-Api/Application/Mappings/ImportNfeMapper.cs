using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Entities;

namespace NfeExplorer_Api.Application.Mappings;

public static class ImportNfeMapper
{
    public static ImportNfeResponse ToImportNfeResponse(Invoice invoice)
    {
        return new ImportNfeResponse
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            Issuer = invoice.Issuer.TradeName ?? invoice.Issuer.LegalName,
            TotalAmount = invoice.TotalAmount,
            InvoiceType = invoice.InvoiceType
        };
    }
}
