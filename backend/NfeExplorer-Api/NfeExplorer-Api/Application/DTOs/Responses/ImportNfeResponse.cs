using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.DTOs.Responses;

public class ImportNfeResponse
{
    public NfeResponse Nfe { get; set; }
}

public class NfeResponse
{
    public Guid Id { get; set; }
    public string ChaveAcesso { get; set; }
    public string NaturezaOperacao { get; set; }
    public string NumeroNota { get; set; }
    public string Serie { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorPago { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public TipoNota TipoNota { get; set; }
    
    public DateTime DataEmissao { get; set; }
    
    public EmitenteResponse Emitente { get; set; }
    public DestinatarioResponse Destinatario { get; set; }
    public IEnumerable<ProdutoResponse> Produtos { get; set; }
    public ImpostosNfeResponse Impostos { get; set; }
    public TransportadoraResponse Transportadora { get; set; }
}

public class EmitenteResponse
{
    public string RazaoSocial { get; set; }
    public string? NomeFantasia { get; set; }
    public string CNPJ { get; set; }
    public string? InscricaoEstadual { get; set; }
    public required string Municipio { get; set; }
    public required string UF { get; set; }
    public required string CEP { get; set; }
}

public class DestinatarioResponse
{
    public required string RazaoSocial { get; set; }
    public string? CNPJ { get; set; }
    public string? CPF { get; set; }
    public string? InscricaoEstadual { get; set; }
    public required string Municipio { get; set; }
    public required string CEP { get; set; }
}

public class ProdutoResponse
{
    public Guid Id { get; set; }
    public required string Descricao { get; set; }
    public required string NCM { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}

public class ImpostosNfeResponse
{
    public decimal ValorProdutos { get; set; }
    public decimal BaseCalculoICMS { get; set; }
    public decimal ValorICMS { get; set; }
    public decimal ValorPIS { get; set; }
    public decimal ValorCOFINS { get; set; }
    public decimal ValorTotalTributos { get; set; }
    public decimal ValorNota { get; set; }
    public decimal AliquotaIcms { get; set; }
}

public class TransportadoraResponse
{
    public Guid Id { get; set; }
    public required string RazaoSocial { get; set; }
    public string? CNPJ { get; set; }
    public string? CPF { get; set; }
    public string? InscricaoEstadual { get; set; }
    public string? Municipio { get; set; }
    public string? UF { get; set; }
    public ModalidadeFrete ModalidadeFrete { get; set; }
}