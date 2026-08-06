using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Domain.Entities;

namespace NfeExplorer_Api.Domain.Interfaces;

public interface INotaFiscalRepository
{
    Task<NotaFiscal?> GetByIdAsync(Guid id);
    Task<NotaFiscal?> GetByChaveAsync(string chave);
    Task AddAsync(NotaFiscal notaFiscal);
    Task<bool> DeleteAsync(Guid id);

    Task<(IReadOnlyList<NotaFiscal> Items, int Total)> GetAllAsync(NfeListRequest filter);
    Task<IReadOnlyList<string>> GetEmitentesAsync();

    Task<int> CountAsync();
    Task<decimal> SumValorTotalAsync();
    Task<int> CountByTipoAsync(Domain.Enums.TipoNota tipo);
    Task<IReadOnlyList<(string Nome, decimal Valor)>> GetTopEmitentesAsync(int take);
    Task<IReadOnlyList<(int Ano, int Mes, decimal Valor)>> GetValorPorMesAsync();

    Task AddImportLogAsync(ImportLog log);
    Task<IReadOnlyList<ImportLog>> GetImportLogsAsync(Domain.Enums.StatusImportacao? status);
}
