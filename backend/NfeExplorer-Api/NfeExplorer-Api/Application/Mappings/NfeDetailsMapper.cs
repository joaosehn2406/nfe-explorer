using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Entities;

namespace NfeExplorer_Api.Application.Mappings;

public static class NfeDetailsMapper
{
    public static NfeDetailsResponse ToResponse(Invoice invoice)
    {
        return new NfeDetailsResponse
        {
            Nfe = new NfeResponse
            {
                Id = invoice.Id,
                AccessKey = invoice.AccessKey,
                OperationNature = invoice.OperationNature,
                InvoiceNumber = invoice.InvoiceNumber,
                Series = invoice.Series,
                TotalAmount = invoice.TotalAmount,
                PaidAmount = invoice.PaidAmount,
                PaymentMethod = invoice.PaymentMethod,
                InvoiceType = invoice.InvoiceType,
                IssuedAt = invoice.IssuedAt,
                Issuer = new IssuerResponse
                {
                    LegalName = invoice.Issuer.LegalName,
                    TradeName = invoice.Issuer.TradeName,
                    CNPJ = invoice.Issuer.CNPJ,
                    StateRegistration = invoice.Issuer.StateRegistration,
                    City = invoice.Issuer.City,
                    UF = invoice.Issuer.UF,
                    ZipCode = invoice.Issuer.ZipCode
                },
                Recipient = new RecipientResponse
                {
                    LegalName = invoice.Recipient.LegalName,
                    CNPJ = invoice.Recipient.CNPJ,
                    CPF = invoice.Recipient.CPF,
                    StateRegistration = invoice.Recipient.StateRegistration,
                    City = invoice.Recipient.City,
                    ZipCode = invoice.Recipient.ZipCode
                },
                Carrier = invoice.Carrier == null
                    ? null
                    : new CarrierResponse
                    {
                        Id = invoice.Carrier.Id,
                        LegalName = invoice.Carrier.LegalName,
                        CNPJ = invoice.Carrier.CNPJ,
                        CPF = invoice.Carrier.CPF,
                        StateRegistration = invoice.Carrier.StateRegistration,
                        City = invoice.Carrier.City,
                        UF = invoice.Carrier.UF,
                        FreightMode = invoice.Carrier.FreightMode
                    },
                Taxes = new NfeTaxesResponse
                {
                    ProductAmount = invoice.Taxes?.ProductAmount ?? 0,
                    IcmsTaxBase = invoice.Taxes?.IcmsTaxBase ?? 0,
                    IcmsAmount = invoice.Taxes?.IcmsAmount ?? 0,
                    PisAmount = invoice.Taxes?.PisAmount ?? 0,
                    CofinsAmount = invoice.Taxes?.CofinsAmount ?? 0,
                    TotalTaxesAmount = invoice.Taxes?.TotalTaxesAmount ?? 0,
                    InvoiceAmount = invoice.Taxes?.InvoiceAmount ?? invoice.TotalAmount,
                    IcmsRate = invoice.Taxes?.IcmsRate ?? 0
                },
                Products = invoice.Products.Select(product => new ProductResponse
                {
                    Id = product.Id,
                    Description = product.Description,
                    NCM = product.NCM,
                    Quantity = product.Quantity,
                    UnitAmount = product.UnitAmount,
                    TotalAmount = product.TotalAmount
                }).ToList()
            }
        };
    }
}
