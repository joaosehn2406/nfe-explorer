using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Entities;

namespace NfeExplorer_Api.Application.Mappings;

public class ImportNfeMapper
{
    public static ImportNfeResponse ToImportNfeResponse(NotaFiscal notaFiscal)
    {
        return new ImportNfeResponse
        {
            Id = notaFiscal.Id,
            NumeroNota = notaFiscal.NumeroNota,
            Emitente = notaFiscal.Emitente.NomeFantasia ?? notaFiscal.Emitente.RazaoSocial,
            ValorTotal = notaFiscal.ValorTotal,
            TipoNota = notaFiscal.TipoNota,
        };
    }
}