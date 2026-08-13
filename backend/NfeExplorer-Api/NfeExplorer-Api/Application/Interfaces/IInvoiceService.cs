using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.Interfaces;

public interface IInvoiceService
{
    Task<ImportNfeResponse> AddAsync(ParseNfeRequest request);
    Task<NfeDetailsResponse?> GetByIdAsync(Guid id);
    Task<NfeDetailsResponse?> GetByAccessKeyAsync(string accessKey);
    Task<bool> DeleteAsync(Guid id);
    Task<PagedResponse<NfeListItemResponse>> GetAllAsync(NfeListRequest filter);
    Task<IReadOnlyList<string>> GetIssuersAsync();
    Task<DashboardStatsResponse> GetDashboardAsync();
    Task<IReadOnlyList<ImportLogResponse>> GetHistoryAsync(ImportStatus? status);
}
