using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Entities;

namespace NfeExplorer_Api.Application.Mappings;

public static class NfeListMapper
{
    public static NfeListItemResponse ToListItem(NotaFiscal nota)
    {
        return new NfeListItemResponse
        {
            Id = nota.Id,
            NumeroNota = nota.NumeroNota,
            Serie = nota.Serie,
            ChaveAcesso = nota.ChaveAcesso,
            TipoNota = nota.TipoNota,
            DataEmissao = nota.DataEmissao,
            ValorTotal = nota.ValorTotal,
            EmitenteNome = nota.Emitente.NomeFantasia ?? nota.Emitente.RazaoSocial,
            EmitenteCnpj = nota.Emitente.CNPJ,
            DestinatarioNome = nota.Destinatario.RazaoSocial
        };
    }
}
