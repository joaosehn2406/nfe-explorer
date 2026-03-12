namespace NfeExplorer_Api.Domain.Enums;

public enum FormaPagamento
{
    Dinheiro = 01,
    Cheque = 02,
    CartaoCredito = 03,
    CartaoDebito = 04,
    CreditoLoja = 05,
    ValeAlimentacao = 10,
    ValeRefeicao = 11,
    ValePresente = 12,
    ValeCombustivel = 13,
    Boleto = 15,
    SemPagamento = 90,
    Outros = 99
}