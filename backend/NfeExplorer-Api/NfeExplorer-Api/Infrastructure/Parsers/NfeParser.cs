using System.Xml.Linq;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Infrastructure.Parsers;

public static class NfeParser
{
    private static string? Get(XElement? element, string localName)
    {
        return element?.Elements()
            .FirstOrDefault(e => e.Name.LocalName == localName)?.Value;
    }

    public static NotaFiscal Parse(string xml)
    {
        var document = XDocument.Parse(xml);
        var infNfe = document.Descendants()
            .FirstOrDefault(info => info.Name.LocalName == "infNFe");

        var ide = document.Descendants()
            .FirstOrDefault(info => info.Name.LocalName == "ide");

        var pag = document.Descendants()
            .FirstOrDefault(info => info.Name.LocalName == "detPag");

        return new NotaFiscal
        {
            ChaveAcesso = infNfe?.Attribute("Id")?.Value?.Replace("NFe", ""),
            DataEmissao = DateTime.Parse(Get(ide, "dhEmi") ?? throw new ArgumentException("dhEmi ausente no XML.")),
            DataImportacao = DateTime.UtcNow,
            NaturezaOperacao = Get(ide, "natOp"),
            NumeroNota = Get(ide, "nNF"),
            Serie = Get(ide, "serie"),
            ValorTotal = decimal.Parse(document.Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "vNF")?.Value ?? "0"),
            ValorPago = decimal.Parse(Get(pag, "vPag") ?? "0"),
            FormaPagamento = (FormaPagamento)int.Parse(Get(pag, "tPag") ?? "99"),
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
        var emit = infNFe.Descendants()
            .FirstOrDefault(emit => emit.Name.LocalName == "emit");

        var enderEmit = emit?.Descendants()
            .FirstOrDefault(enderEmit => enderEmit.Name.LocalName == "enderEmit");

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
        var dest = infNFe.Descendants()
            .FirstOrDefault(dest => dest.Name.LocalName == "dest");

        var enderDest = dest?.Descendants()
            .FirstOrDefault(enderDest => enderDest.Name.LocalName == "enderDest");

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
        var transp = infNFe.Descendants()
            .FirstOrDefault(t => t.Name.LocalName == "transporta");

        if (transp == null) return null;

        var modFrete = infNFe.Descendants()
            .FirstOrDefault(t => t.Name.LocalName == "transp");

        return new Transportadora
        {
            RazaoSocial = Get(transp, "xNome"),
            CNPJ = Get(transp, "CNPJ"),
            CPF = Get(transp, "CPF"),
            InscricaoEstadual = Get(transp, "IE"),
            Municipio = Get(transp, "xMun"),
            UF = Get(transp, "UF"),
            ModalidadeFrete = (ModalidadeFrete)int.Parse(Get(modFrete, "modFrete") ?? "9")
        };
    }

    private static ImpostosNfe ParseImpostos(XElement infNFe)
    {
        var icmsTot = infNFe.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "ICMSTot");

        var valorICMS = decimal.Parse(Get(icmsTot, "vICMS") ?? "0");
        var baseCalculo = decimal.Parse(Get(icmsTot, "vBC") ?? "1");

        return new ImpostosNfe
        {
            ValorProdutos = decimal.Parse(Get(icmsTot, "vProd") ?? "0"),
            BaseCalculoICMS = decimal.Parse(Get(icmsTot, "vBC") ?? "0"),
            ValorICMS = decimal.Parse(Get(icmsTot, "vICMS") ?? "0"),
            ValorPIS = decimal.Parse(Get(icmsTot, "vPIS") ?? "0"),
            ValorCOFINS = decimal.Parse(Get(icmsTot, "vCOFINS") ?? "0"),
            AliquotaIcms = baseCalculo != 0 ? (valorICMS / baseCalculo) * 100 : 0,
            ValorTotalTributos = decimal.Parse(Get(icmsTot, "vTribFed") ?? "0"),
            ValorNota = decimal.Parse(Get(icmsTot, "vNF") ?? "0")
        };
    }

    private static List<Produto> ParseProdutos(XElement infNFe)
    {
        return infNFe.Descendants()
            .Where(e => e.Name.LocalName == "det")
            .Select(det =>
            {
                var prod = det.Descendants()
                    .FirstOrDefault(e => e.Name.LocalName == "prod");

                return new Produto
                {
                    CodigoProduto = Get(prod, "cProd"),
                    Descricao = Get(prod, "xProd"),
                    NCM = Get(prod, "NCM"),
                    Quantidade = decimal.Parse(Get(prod, "qCom") ?? "0"),
                    ValorUnitario = decimal.Parse(Get(prod, "vUnCom") ?? "0"),
                    ValorTotal = decimal.Parse(Get(prod, "vProd") ?? "0")
                };
            })
            .ToList();
    }
}