using System.Globalization;
using System.Xml.Linq;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Infrastructure.Parsers;

public static class NfeParser
{
    private static XElement? Find(XContainer? container, string localName) =>
        container?.Descendants().FirstOrDefault(element => element.Name.LocalName == localName);

    private static string? Get(XElement? element, string localName) =>
        element?.Elements().FirstOrDefault(child => child.Name.LocalName == localName)?.Value;

    private static string RequiredGet(XElement? element, string localName)
    {
        return Get(element, localName) ?? throw new ArgumentException($"{localName} is missing from XML.");
    }

    private static decimal ParseDecimal(string? value, decimal fallback = 0m) =>
        decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : fallback;

    private static int ParseInt(string? value, int fallback) =>
        int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : fallback;

    public static Invoice Parse(string xml)
    {
        var document = XDocument.Parse(xml);
        var infNfe = Find(document, "infNFe");
        var ide = Find(document, "ide");
        var payment = Find(document, "detPag");

        if (infNfe == null)
        {
            throw new ArgumentException("infNFe is missing from XML.");
        }

        return new Invoice
        {
            AccessKey = infNfe.Attribute("Id")?.Value?.Replace("NFe", "")
                ?? throw new ArgumentException("NF-e access key is missing from XML."),
            IssuedAt = DateTime.Parse(RequiredGet(ide, "dhEmi"),
                CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            ImportedAt = DateTime.UtcNow,
            OperationNature = RequiredGet(ide, "natOp"),
            InvoiceNumber = RequiredGet(ide, "nNF"),
            Series = RequiredGet(ide, "serie"),
            TotalAmount = ParseDecimal(Find(document, "vNF")?.Value),
            PaidAmount = ParseDecimal(Get(payment, "vPag")),
            PaymentMethod = (PaymentMethod)ParseInt(Get(payment, "tPag"), 99),
            InvoiceType = Get(ide, "tpNF") == "1" ? InvoiceType.Outbound : InvoiceType.Inbound,
            Issuer = ParseIssuer(infNfe),
            Recipient = ParseRecipient(infNfe),
            Carrier = ParseCarrier(infNfe),
            Taxes = ParseTaxes(infNfe),
            Products = ParseProducts(infNfe)
        };
    }

    private static Issuer ParseIssuer(XElement infNFe)
    {
        var issuer = Find(infNFe, "emit");
        var issuerAddress = Find(issuer, "enderEmit");

        return new Issuer
        {
            LegalName = RequiredGet(issuer, "xNome"),
            TradeName = Get(issuer, "xFant"),
            CNPJ = RequiredGet(issuer, "CNPJ"),
            StateRegistration = Get(issuer, "IE"),
            Street = RequiredGet(issuerAddress, "xLgr"),
            Number = RequiredGet(issuerAddress, "nro"),
            District = RequiredGet(issuerAddress, "xBairro"),
            City = RequiredGet(issuerAddress, "xMun"),
            UF = RequiredGet(issuerAddress, "UF"),
            ZipCode = RequiredGet(issuerAddress, "CEP")
        };
    }

    private static Recipient ParseRecipient(XElement infNFe)
    {
        var recipient = Find(infNFe, "dest");
        var recipientAddress = Find(recipient, "enderDest");

        return new Recipient
        {
            LegalName = RequiredGet(recipient, "xNome"),
            CNPJ = Get(recipient, "CNPJ"),
            CPF = Get(recipient, "CPF"),
            StateRegistration = Get(recipient, "IE"),
            Street = RequiredGet(recipientAddress, "xLgr"),
            Number = RequiredGet(recipientAddress, "nro"),
            District = RequiredGet(recipientAddress, "xBairro"),
            City = RequiredGet(recipientAddress, "xMun"),
            UF = RequiredGet(recipientAddress, "UF"),
            ZipCode = RequiredGet(recipientAddress, "CEP")
        };
    }

    private static Carrier? ParseCarrier(XElement infNFe)
    {
        var carrier = Find(infNFe, "transporta");

        if (carrier == null)
        {
            return null;
        }

        var shipping = Find(infNFe, "transp");

        return new Carrier
        {
            LegalName = RequiredGet(carrier, "xNome"),
            CNPJ = Get(carrier, "CNPJ"),
            CPF = Get(carrier, "CPF"),
            StateRegistration = Get(carrier, "IE"),
            City = Get(carrier, "xMun"),
            UF = Get(carrier, "UF"),
            FreightMode = (FreightMode)ParseInt(Get(shipping, "modFrete"), 9)
        };
    }

    private static NfeTaxes ParseTaxes(XElement infNFe)
    {
        var icmsTotal = Find(infNFe, "ICMSTot");

        var icmsAmount = ParseDecimal(Get(icmsTotal, "vICMS"));
        var icmsTaxBase = ParseDecimal(Get(icmsTotal, "vBC"));

        return new NfeTaxes
        {
            ProductAmount = ParseDecimal(Get(icmsTotal, "vProd")),
            IcmsTaxBase = icmsTaxBase,
            IcmsAmount = icmsAmount,
            PisAmount = ParseDecimal(Get(icmsTotal, "vPIS")),
            CofinsAmount = ParseDecimal(Get(icmsTotal, "vCOFINS")),
            IcmsRate = icmsTaxBase != 0 ? (icmsAmount / icmsTaxBase) * 100 : 0,
            TotalTaxesAmount = ParseDecimal(Get(icmsTotal, "vTribFed")),
            InvoiceAmount = ParseDecimal(Get(icmsTotal, "vNF"))
        };
    }

    private static List<Product> ParseProducts(XElement infNFe)
    {
        return infNFe.Descendants()
            .Where(element => element.Name.LocalName == "det")
            .Select(detail =>
            {
                var product = Find(detail, "prod");

                return new Product
                {
                    ProductCode = RequiredGet(product, "cProd"),
                    Description = RequiredGet(product, "xProd"),
                    NCM = RequiredGet(product, "NCM"),
                    Quantity = ParseDecimal(Get(product, "qCom")),
                    UnitAmount = ParseDecimal(Get(product, "vUnCom")),
                    TotalAmount = ParseDecimal(Get(product, "vProd"))
                };
            })
            .ToList();
    }
}
