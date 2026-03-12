using NfeExplorer_Api.Dto;
using NfeExplorer_Api.Parser;
using NfeExplorer_Api.Repository;
using NfeExplorer_Api.Validator;

namespace NfeExplorer_Api.Application.Service;

public class NotaFiscalService : INotaFiscalService
{
    private readonly INotaFiscalRepository _repository;

    public NotaFiscalService(INotaFiscalRepository repository)
    {
        _repository = repository;
    }

    public async Task<ImportNfeResponse> AddAsync(ParseNfeRequest request)
    {
        NFeValidator.ValidarRequest(request);

        string xml;

        if (request.File != null)
        {
            using var stream = request.File.OpenReadStream();
            using var reader = new StreamReader(stream);
            xml = await reader.ReadToEndAsync();
        }
        else
        {
            xml = request.XmlText!;
        }

        NFeValidator.ValidarXml(xml);

        var notaFiscal = NfeParser.Parse(xml);

        var notaExistente = await _repository.GetByChaveAsync(notaFiscal.ChaveAcesso);
        if (notaExistente != null)
            throw new ArgumentException("Nota fiscal já importada. Chave de acesso duplicada.");

        await _repository.AddAsync(notaFiscal);

        var emitente = new EmitenteResponse
        {
            RazaoSocial = notaFiscal.Emitente.RazaoSocial,
            NomeFantasia = notaFiscal.Emitente.NomeFantasia,
            CNPJ = notaFiscal.Emitente.CNPJ,
            InscricaoEstadual = notaFiscal.Emitente.InscricaoEstadual,
            Municipio = notaFiscal.Emitente.Municipio,
            UF = notaFiscal.Emitente.UF,
            CEP = notaFiscal.Emitente.CEP
        };

        var destinatario = new DestinatarioResponse
        {
            RazaoSocial = notaFiscal.Destinatario.RazaoSocial,
            CNPJ = notaFiscal.Destinatario.CNPJ,
            CPF = notaFiscal.Destinatario.CPF,
            InscricaoEstadual = notaFiscal.Destinatario.InscricaoEstadual,
            Municipio = notaFiscal.Destinatario.Municipio,
            CEP = notaFiscal.Destinatario.CEP
        };

        TransportadoraResponse? transportadora = null;
        if (notaFiscal.Transportadora != null)
        {
            transportadora = new TransportadoraResponse
            {
                Id = notaFiscal.Transportadora.Id,
                RazaoSocial = notaFiscal.Transportadora.RazaoSocial,
                CNPJ = notaFiscal.Transportadora.CNPJ,
                CPF = notaFiscal.Transportadora.CPF,
                InscricaoEstadual = notaFiscal.Transportadora.InscricaoEstadual,
                Municipio = notaFiscal.Transportadora.Municipio,
                UF = notaFiscal.Transportadora.UF,
                ModalidadeFrete = notaFiscal.Transportadora.ModalidadeFrete
            };
        }

        var impostos = new ImpostosNfeResponse
        {
            ValorProdutos = notaFiscal.ImpostosNfe.ValorProdutos,
            BaseCalculoICMS = notaFiscal.ImpostosNfe.BaseCalculoICMS,
            ValorICMS = notaFiscal.ImpostosNfe.ValorICMS,
            ValorPIS = notaFiscal.ImpostosNfe.ValorPIS,
            ValorCOFINS = notaFiscal.ImpostosNfe.ValorCOFINS,
            ValorTotalTributos = notaFiscal.ImpostosNfe.ValorTotalTributos,
            ValorNota = notaFiscal.ImpostosNfe.ValorNota,
            AliquotaIcms = notaFiscal.ImpostosNfe.AliquotaIcms
        };

        var produtos = notaFiscal.Produtos.Select(p => new ProdutoResponse
        {
            Id = p.Id,
            Descricao = p.Descricao,
            NCM = p.NCM,
            Quantidade = p.Quantidade,
            ValorUnitario = p.ValorUnitario,
            ValorTotal = p.ValorTotal
        });

        return new ImportNfeResponse
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
                Emitente = emitente,
                Destinatario = destinatario,
                Transportadora = transportadora,
                Impostos = impostos,
                Produtos = produtos
            }
        };
    }

    public async Task<ImportNfeResponse?> GetByIdAsync(Guid id)
    {
        var notaFiscal = await _repository.GetByIdAsync(id);

        if (notaFiscal == null)
            return null;

        TransportadoraResponse? transportadora = null;
        if (notaFiscal.Transportadora != null)
        {
            transportadora = new TransportadoraResponse
            {
                Id = notaFiscal.Transportadora.Id,
                RazaoSocial = notaFiscal.Transportadora.RazaoSocial,
                CNPJ = notaFiscal.Transportadora.CNPJ,
                CPF = notaFiscal.Transportadora.CPF,
                InscricaoEstadual = notaFiscal.Transportadora.InscricaoEstadual,
                Municipio = notaFiscal.Transportadora.Municipio,
                UF = notaFiscal.Transportadora.UF,
                ModalidadeFrete = notaFiscal.Transportadora.ModalidadeFrete
            };
        }

        return new ImportNfeResponse
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
                Transportadora = transportadora,
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
                Produtos = notaFiscal.Produtos.Select(p => new ProdutoResponse
                {
                    Id = p.Id,
                    Descricao = p.Descricao,
                    NCM = p.NCM,
                    Quantidade = p.Quantidade,
                    ValorUnitario = p.ValorUnitario,
                    ValorTotal = p.ValorTotal
                })
            }
        };
    }

    public async Task<ImportNfeResponse?> GetByChaveAsync(string chave)
    {
        var notaFiscal = await _repository.GetByChaveAsync(chave);

        if (notaFiscal == null)
            return null;

        TransportadoraResponse? transportadora = null;
        if (notaFiscal.Transportadora != null)
        {
            transportadora = new TransportadoraResponse
            {
                Id = notaFiscal.Transportadora.Id,
                RazaoSocial = notaFiscal.Transportadora.RazaoSocial,
                CNPJ = notaFiscal.Transportadora.CNPJ,
                CPF = notaFiscal.Transportadora.CPF,
                InscricaoEstadual = notaFiscal.Transportadora.InscricaoEstadual,
                Municipio = notaFiscal.Transportadora.Municipio,
                UF = notaFiscal.Transportadora.UF,
                ModalidadeFrete = notaFiscal.Transportadora.ModalidadeFrete
            };
        }

        return new ImportNfeResponse
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
                Transportadora = transportadora,
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
                Produtos = notaFiscal.Produtos.Select(p => new ProdutoResponse
                {
                    Id = p.Id,
                    Descricao = p.Descricao,
                    NCM = p.NCM,
                    Quantidade = p.Quantidade,
                    ValorUnitario = p.ValorUnitario,
                    ValorTotal = p.ValorTotal
                })
            }
        };
    }
}