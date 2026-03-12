using NfeExplorer_Api.Dto;

namespace NfeExplorer_Api.Application.Service;

public interface INotaFiscalService
{
    Task<ImportNfeResponse> AddAsync(ParseNfeRequest request);
    Task<ImportNfeResponse?> GetByIdAsync(Guid id);
    Task<ImportNfeResponse?> GetByChaveAsync(string chave);
}