using NfeExplorer_Api.Domain.Model;

namespace NfeExplorer_Api.Repository;

public interface INotaFiscalRepository
{
    Task<NotaFiscal?> GetByIdAsync(Guid id);
    Task<NotaFiscal?> GetByChaveAsync(string chave);
    Task AddAsync(NotaFiscal notaFiscal);
}