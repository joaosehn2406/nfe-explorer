using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.Interfaces;

public interface INotaFiscalService
{
    Task<ImportNfeResponse> AddAsync(ParseNfeRequest request);
    Task<NfeDetailsResponse?> GetByIdAsync(Guid id);
    Task<NfeDetailsResponse?> GetByChaveAsync(string chave);
    Task<bool> DeleteAsync(Guid id);

    Task<PagedResponse<NfeListItemResponse>> GetAllAsync(NfeListRequest filter);
    Task<IReadOnlyList<string>> GetEmitentesAsync();
    Task<DashboardStatsResponse> GetDashboardAsync();
    Task<IReadOnlyList<ImportLogResponse>> GetHistoricoAsync(StatusImportacao? status);
}
