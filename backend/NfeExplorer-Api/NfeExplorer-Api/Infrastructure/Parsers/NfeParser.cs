using System.Globalization;
using System.Xml.Linq;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Infrastructure.Parsers;

public static class NfeParser
{
    private static XElement? Find(XContainer? container, string localName) =>
        container?.Descendants().FirstOrDefault(e => e.Name.LocalName == localName);

    private static string? Get(XElement? element, string localName) =>
        element?.Elements().FirstOrDefault(e => e.Name.LocalName == localName)?.Value;

    private static decimal ParseDecimal(string? value, decimal fallback = 0m) =>
        decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : fallback;

    private static int ParseInt(string? value, int fallback) =>
        int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : fallback;

    public static NotaFiscal Parse(string xml)
    {
        var document = XDocument.Parse(xml);
        var infNfe = Find(document, "infNFe");
        var ide = Find(document, "ide");
        var pag = Find(document, "detPag");

        return new NotaFiscal
        {
            ChaveAcesso = infNfe?.Attribute("Id")?.Value?.Replace("NFe", ""),
            DataEmissao = DateTime.Parse(Get(ide, "dhEmi") ?? throw new ArgumentException("dhEmi ausente no XML."),
                CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            DataImportacao = DateTime.UtcNow,
            NaturezaOperacao = Get(ide, "natOp"),
            NumeroNota = Get(ide, "nNF"),
            Serie = Get(ide, "serie"),
            ValorTotal = ParseDecimal(Find(document, "vNF")?.Value),
            ValorPago = ParseDecimal(Get(pag, "vPag")),
            FormaPagamento = (FormaPagamento)ParseInt(Get(pag, "tPag"), 99),
            TipoNota = Get(ide, "tpNF") == "1" ? TipoNota.Saida : TipoNota.Entrada,
            Emitente = ParseEmitente(infNfe),
            Destinatario = ParseDestinatario(infNfe),
            Transportadora = ParseTransportadora(infNfe),
            ImpostosNfe = ParseImpostos(infNfe),
            Produtos = ParseProdutos(infNfe)
        };
    }

    private static Emitente ParseEmitente(XElement infNFe)
    {
        var emit = Find(infNFe, "emit");
        var enderEmit = Find(emit, "enderEmit");

        return new Emitente
        {
            RazaoSocial = Get(emit, "xNome"),
            NomeFantasia = Get(emit, "xFant"),
            CNPJ = Get(emit, "CNPJ"),
            InscricaoEstadual = Get(emit, "IE"),
            Logradouro = Get(enderEmit, "xLgr"),
            Numero = Get(enderEmit, "nro"),
            Bairro = Get(enderEmit, "xBairro"),
            Municipio = Get(enderEmit, "xMun"),
            UF = Get(enderEmit, "UF"),
            CEP = Get(enderEmit, "CEP")
        };
    }

    private static Destinatario ParseDestinatario(XElement infNFe)
    {
        var dest = Find(infNFe, "dest");
        var enderDest = Find(dest, "enderDest");

        return new Destinatario
        {
            RazaoSocial = Get(dest, "xNome"),
            CNPJ = Get(dest, "CNPJ"),
            CPF = Get(dest, "CPF"),
            InscricaoEstadual = Get(dest, "IE"),
            Logradouro = Get(enderDest, "xLgr"),
            Numero = Get(enderDest, "nro"),
            Bairro = Get(enderDest, "xBairro"),
            Municipio = Get(enderDest, "xMun"),
            UF = Get(enderDest, "UF"),
            CEP = Get(enderDest, "CEP")
        };
    }

    private static Transportadora? ParseTransportadora(XElement infNFe)
    {
        var transp = Find(infNFe, "transporta");

        if (transp == null) return null;

        var modFrete = Find(infNFe, "transp");

        return new Transportadora
        {
            RazaoSocial = Get(transp, "xNome"),
            CNPJ = Get(transp, "CNPJ"),
            CPF = Get(transp, "CPF"),
            InscricaoEstadual = Get(transp, "IE"),
            Municipio = Get(transp, "xMun"),
            UF = Get(transp, "UF"),
            ModalidadeFrete = (ModalidadeFrete)ParseInt(Get(modFrete, "modFrete"), 9)
        };
    }

    private static ImpostosNfe ParseImpostos(XElement infNFe)
    {
        var icmsTot = Find(infNFe, "ICMSTot");

        var valorICMS = ParseDecimal(Get(icmsTot, "vICMS"));
        var baseCalculo = ParseDecimal(Get(icmsTot, "vBC"));

        return new ImpostosNfe
        {
            ValorProdutos = ParseDecimal(Get(icmsTot, "vProd")),
            BaseCalculoICMS = baseCalculo,
            ValorICMS = valorICMS,
            ValorPIS = ParseDecimal(Get(icmsTot, "vPIS")),
            ValorCOFINS = ParseDecimal(Get(icmsTot, "vCOFINS")),
            AliquotaIcms = baseCalculo != 0 ? (valorICMS / baseCalculo) * 100 : 0,
            ValorTotalTributos = ParseDecimal(Get(icmsTot, "vTribFed")),
            ValorNota = ParseDecimal(Get(icmsTot, "vNF"))
        };
    }

    private static List<Produto> ParseProdutos(XElement infNFe)
    {
        return infNFe.Descendants()
            .Where(e => e.Name.LocalName == "det")
            .Select(det =>
            {
                var prod = Find(det, "prod");

                return new Produto
                {
                    CodigoProduto = Get(prod, "cProd"),
                    Descricao = Get(prod, "xProd"),
                    NCM = Get(prod, "NCM"),
                    Quantidade = ParseDecimal(Get(prod, "qCom")),
                    ValorUnitario = ParseDecimal(Get(prod, "vUnCom")),
                    ValorTotal = ParseDecimal(Get(prod, "vProd"))
                };
            })
            .ToList();
    }
}