using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Entities;

namespace NfeExplorer_Api.Application.Mappings;

public static class NfeDetailsMapper
{
    public static NfeDetailsResponse ToResponse(NotaFiscal notaFiscal)
    {
        return new NfeDetailsResponse
        {
            Nfe = new NfeResponse
            {
                Id = notaFiscal.Id,
                ChaveAcesso = notaFiscal.ChaveAcesso,
                NaturezaOperacao = notaFiscal.NaturezaOperacao,
                NumeroNota = notaFiscal.NumeroNota,
                Serie = notaFiscal.Serie,
                ValorTotal = notaFiscal.ValorTotal,
                ValorPago = notaFiscal.ValorPago,
                FormaPagamento = notaFiscal.FormaPagamento,
                TipoNota = notaFiscal.TipoNota,
                DataEmissao = notaFiscal.DataEmissao,
                Emitente = new EmitenteResponse
                {
                    RazaoSocial = notaFiscal.Emitente.RazaoSocial,
                    NomeFantasia = notaFiscal.Emitente.NomeFantasia,
                    CNPJ = notaFiscal.Emitente.CNPJ,
                    InscricaoEstadual = notaFiscal.Emitente.InscricaoEstadual,
                    Municipio = notaFiscal.Emitente.Municipio,
                    UF = notaFiscal.Emitente.UF,
                    CEP = notaFiscal.Emitente.CEP
                },
                Destinatario = new DestinatarioResponse
                {
                    RazaoSocial = notaFiscal.Destinatario.RazaoSocial,
                    CNPJ = notaFiscal.Destinatario.CNPJ,
                    CPF = notaFiscal.Destinatario.CPF,
                    InscricaoEstadual = notaFiscal.Destinatario.InscricaoEstadual,
                    Municipio = notaFiscal.Destinatario.Municipio,
                    CEP = notaFiscal.Destinatario.CEP
                },
                Transportadora = new TransportadoraResponse
                {
                    Id = notaFiscal.Transportadora.Id,
                    RazaoSocial = notaFiscal.Transportadora.RazaoSocial,
                    CNPJ = notaFiscal.Transportadora.CNPJ,
                    CPF = notaFiscal.Transportadora.CPF,
                    InscricaoEstadual = notaFiscal.Transportadora.InscricaoEstadual,
                    Municipio = notaFiscal.Transportadora.Municipio,
                    UF = notaFiscal.Transportadora.UF,
                    ModalidadeFrete = notaFiscal.Transportadora.ModalidadeFrete
                },
                Impostos = new ImpostosNfeResponse
                {
                    ValorProdutos = notaFiscal.ImpostosNfe.ValorProdutos,
                    BaseCalculoICMS = notaFiscal.ImpostosNfe.BaseCalculoICMS,
                    ValorICMS = notaFiscal.ImpostosNfe.ValorICMS,
                    ValorPIS = notaFiscal.ImpostosNfe.ValorPIS,
                    ValorCOFINS = notaFiscal.ImpostosNfe.ValorCOFINS,
                    ValorTotalTributos = notaFiscal.ImpostosNfe.ValorTotalTributos,
                    ValorNota = notaFiscal.ImpostosNfe.ValorNota,
                    AliquotaIcms = notaFiscal.ImpostosNfe.AliquotaIcms
                },
                Produtos = notaFiscal.Produtos.Select(produto => new ProdutoResponse
                {
                    Id = produto.Id,
                    Descricao = produto.Descricao,
                    NCM = produto.NCM,
                    Quantidade = produto.Quantidade,
                    ValorUnitario = produto.ValorUnitario,
                    ValorTotal = produto.ValorTotal
                })
            }
        };
    }
}