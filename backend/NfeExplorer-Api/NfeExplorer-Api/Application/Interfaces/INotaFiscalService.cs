using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Application.DTOs.Responses;

namespace NfeExplorer_Api.Application.Interfaces;

public interface INotaFiscalService
{
    Task<ImportNfeResponse> AddAsync(ParseNfeRequest request);
    Task<ImportNfeResponse?> GetByIdAsync(Guid id);
    Task<ImportNfeResponse?> GetByChaveAsync(string chave);
}