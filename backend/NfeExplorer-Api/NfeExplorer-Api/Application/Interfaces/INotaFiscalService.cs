using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Application.DTOs.Responses;

namespace NfeExplorer_Api.Application.Interfaces;

public interface INotaFiscalService
{
    Task<ImportNfeResponse> AddAsync(ParseNfeRequest request);
    Task<NfeDetailsResponse?> GetByIdAsync(Guid id);
    Task<NfeDetailsResponse?> GetByChaveAsync(string chave);
}