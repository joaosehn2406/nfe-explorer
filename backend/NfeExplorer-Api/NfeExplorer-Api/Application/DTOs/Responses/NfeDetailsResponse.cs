using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.DTOs.Responses;

public class NfeDetailsResponse
{
    public NfeResponse Nfe { get; set; } = default!;
}

public class NfeResponse
{
    public Guid Id { get; set; }
    public string AccessKey { get; set; } = string.Empty;
    public string OperationNature { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public string Series { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public InvoiceType InvoiceType { get; set; }
    public DateTime IssuedAt { get; set; }

    public IssuerResponse Issuer { get; set; } = default!;
    public RecipientResponse Recipient { get; set; } = default!;
    public IEnumerable<ProductResponse> Products { get; set; } = new List<ProductResponse>();
    public NfeTaxesResponse Taxes { get; set; } = default!;
    public CarrierResponse? Carrier { get; set; }
}

public class IssuerResponse
{
    public string LegalName { get; set; } = string.Empty;
    public string? TradeName { get; set; }
    public string CNPJ { get; set; } = string.Empty;
    public string? StateRegistration { get; set; }
    public required string City { get; set; }
    public required string UF { get; set; }
    public required string ZipCode { get; set; }
}

public class RecipientResponse
{
    public required string LegalName { get; set; }
    public string? CNPJ { get; set; }
    public string? CPF { get; set; }
    public string? StateRegistration { get; set; }
    public required string City { get; set; }
    public required string ZipCode { get; set; }
}

public class ProductResponse
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
    public required string NCM { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitAmount { get; set; }
    public decimal TotalAmount { get; set; }
}

public class NfeTaxesResponse
{
    public decimal ProductAmount { get; set; }
    public decimal IcmsTaxBase { get; set; }
    public decimal IcmsAmount { get; set; }
    public decimal PisAmount { get; set; }
    public decimal CofinsAmount { get; set; }
    public decimal TotalTaxesAmount { get; set; }
    public decimal InvoiceAmount { get; set; }
    public decimal IcmsRate { get; set; }
}

public class CarrierResponse
{
    public Guid Id { get; set; }
    public required string LegalName { get; set; }
    public string? CNPJ { get; set; }
    public string? CPF { get; set; }
    public string? StateRegistration { get; set; }
    public string? City { get; set; }
    public string? UF { get; set; }
    public FreightMode FreightMode { get; set; }
}
