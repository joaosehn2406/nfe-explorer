using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.Mappings;

public static class ImportLogMapper
{
    public static ImportLog Sucesso(string nomeArquivo, NotaFiscal nota)
    {
        return new ImportLog
        {
            DataHora = DateTime.UtcNow,
            Status = StatusImportacao.Sucesso,
            NomeArquivo = nomeArquivo,
            NumeroNota = nota.NumeroNota,
            Emitente = nota.Emitente.NomeFantasia ?? nota.Emitente.RazaoSocial,
            Valor = nota.ValorTotal
        };
    }

    public static ImportLog Duplicada(string nomeArquivo, NotaFiscal? nota, string mensagem)
    {
        return new ImportLog
        {
            DataHora = DateTime.UtcNow,
            Status = StatusImportacao.Duplicada,
            NomeArquivo = nomeArquivo,
            NumeroNota = nota?.NumeroNota,
            Emitente = nota is null ? null : nota.Emitente.NomeFantasia ?? nota.Emitente.RazaoSocial,
            Mensagem = mensagem
        };
    }

    public static ImportLog Erro(string nomeArquivo, string mensagem)
    {
        return new ImportLog
        {
            DataHora = DateTime.UtcNow,
            Status = StatusImportacao.Erro,
            NomeArquivo = nomeArquivo,
            Mensagem = mensagem
        };
    }

    public static ImportLogResponse ToResponse(ImportLog log)
    {
        return new ImportLogResponse
        {
            Id = log.Id,
            DataHora = log.DataHora,
            Status = log.Status,
            NomeArquivo = log.NomeArquivo,
            NumeroNota = log.NumeroNota,
            Emitente = log.Emitente,
            Valor = log.Valor,
            Mensagem = log.Mensagem
        };
    }
}
