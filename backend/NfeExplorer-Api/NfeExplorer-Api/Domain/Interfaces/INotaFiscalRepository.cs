using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Entities;

namespace NfeExplorer_Api.Domain.Interfaces;

public interface INotaFiscalRepository
{
    Task<NotaFiscal?> GetByIdAsync(Guid id);
    Task<NotaFiscal?> GetByChaveAsync(string chave);
    Task AddAsync(NotaFiscal notaFiscal);
}