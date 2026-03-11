using NfeExplorer_Api.Dto;
using NfeExplorer_Api.Models;

namespace NfeExplorer_Api.Service;

public interface INotaFiscalService
{
    Task<ImportNfeResponse> AddAsync(ParseNfeRequest request);
    Task<ImportNfeResponse?> GetByIdAsync(Guid id);
    Task<ImportNfeResponse?> GetByChaveAsync(string chave);
}