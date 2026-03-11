using System.Xml.Linq;
using NfeExplorer_Api.Models;
using NfeExplorer_Api.Models.Enums;

namespace NfeExplorer_Api.Parser;

public static class NfeParser
{
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
            DataEmissao = DateTime.Parse(ide?.Element("dhEmi")?.Value),
            DataImportacao = DateTime.UtcNow,
            NaturezaOperacao = ide?.Element("natOp")?.Value,
            NumeroNota = ide?.Element("nNF")?.Value,
            Serie = ide?.Element("serie")?.Value,
            ValorTotal = decimal.Parse(document.Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "vNF")?.Value ?? "0"),
            ValorPago = decimal.Parse(pag?.Element("vPag")?.Value ?? "0"),
            FormaPagamento = (FormaPagamento)int.Parse(pag?.Element("tPag")?.Value ?? "99"),
            TipoNota = ide?.Element("tpNF")?.Value == "1" ? TipoNota.Saida : TipoNota.Entrada,
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
            RazaoSocial = emit?.Element("xNome")?.Value,
            NomeFantasia = emit?.Element("xFant")?.Value,
            CNPJ = emit?.Element("CNPJ")?.Value,
            InscricaoEstadual = emit?.Element("IE")?.Value,
            Logradouro = enderEmit?.Element("xLgr")?.Value,
            Numero = enderEmit?.Element("nro")?.Value,
            Bairro = enderEmit?.Element("xBairro")?.Value,
            Municipio = enderEmit?.Element("xMun")?.Value,
            UF = enderEmit?.Element("UF")?.Value,
            CEP = enderEmit?.Element("CEP")?.Value
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
            RazaoSocial = dest?.Element("xNome")?.Value,
            CNPJ = dest?.Element("CNPJ")?.Value,
            CPF = dest?.Element("CPF")?.Value,
            InscricaoEstadual = dest?.Element("IE")?.Value,
            Logradouro = enderDest?.Element("xLgr")?.Value,
            Numero = enderDest?.Element("nro")?.Value,
            Bairro = enderDest?.Element("xBairro")?.Value,
            Municipio = enderDest?.Element("xMun")?.Value,
            UF = enderDest?.Element("UF")?.Value,
            CEP = enderDest?.Element("CEP")?.Value
        };
    }

    private static Transportadora? ParseTransportadora(XElement infNFe)
    {
        var transp = infNFe.Descendants()
            .FirstOrDefault(t => t.Name.LocalName == "transporta");

        if (transp == null) return null;

        var modFrete = infNFe.Descendants()
            .FirstOrDefault(t => t.Name.LocalName == "transp")
            ?.Element("modFrete")?.Value;

        return new Transportadora
        {
            RazaoSocial = transp?.Element("xNome")?.Value,
            CNPJ = transp?.Element("CNPJ")?.Value,
            CPF = transp?.Element("CPF")?.Value,
            InscricaoEstadual = transp?.Element("IE")?.Value,
            Municipio = transp?.Element("xMun")?.Value,
            UF = transp?.Element("UF")?.Value,
            ModalidadeFrete = (ModalidadeFrete)int.Parse(modFrete ?? "9")
        };
    }

    private static ImpostosNfe ParseImpostos(XElement infNFe)
    {
        var icmsTot = infNFe.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "ICMSTot");
        
        var valorICMS = decimal.Parse(icmsTot?.Element("vICMS")?.Value ?? "0");
        var baseCalculo = decimal.Parse(icmsTot?.Element("vBC")?.Value ?? "1");

        return new ImpostosNfe
        {
            ValorProdutos = decimal.Parse(icmsTot?.Element("vProd")?.Value ?? "0"),
            BaseCalculoICMS = decimal.Parse(icmsTot?.Element("vBC")?.Value ?? "0"),
            ValorICMS = decimal.Parse(icmsTot?.Element("vICMS")?.Value ?? "0"),
            ValorPIS = decimal.Parse(icmsTot?.Element("vPIS")?.Value ?? "0"),
            ValorCOFINS = decimal.Parse(icmsTot?.Element("vCOFINS")?.Value ?? "0"),
            AliquotaIcms = baseCalculo != 0 ? (valorICMS / baseCalculo) * 100 : 0,
            ValorTotalTributos = decimal.Parse(icmsTot?.Element("vTribFed")?.Value ?? "0"),
            ValorNota = decimal.Parse(icmsTot?.Element("vNF")?.Value ?? "0")
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
                    CodigoProduto = prod?.Element("cProd")?.Value,
                    Descricao = prod?.Element("xProd")?.Value,
                    NCM = prod?.Element("NCM")?.Value,
                    Quantidade = decimal.Parse(prod?.Element("qCom")?.Value ?? "0"),
                    ValorUnitario = decimal.Parse(prod?.Element("vUnCom")?.Value ?? "0"),
                    ValorTotal = decimal.Parse(prod?.Element("vProd")?.Value ?? "0")
                };
            })
            .ToList();
    }
}