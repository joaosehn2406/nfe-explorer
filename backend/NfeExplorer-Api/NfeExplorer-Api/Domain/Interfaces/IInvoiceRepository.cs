using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Domain.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id);
    Task<Invoice?> GetByAccessKeyAsync(string accessKey);
    Task AddAsync(Invoice invoice);
    Task<bool> DeleteAsync(Guid id);
    Task<(IReadOnlyList<Invoice> Items, int Total)> GetAllAsync(NfeListRequest filter);
    Task<IReadOnlyList<string>> GetIssuersAsync();
    Task<int> CountAsync();
    Task<decimal> SumTotalAmountAsync();
    Task<int> CountByTypeAsync(InvoiceType type);
    Task<IReadOnlyList<(string Name, decimal Amount)>> GetTopIssuersAsync(int take);
    Task<IReadOnlyList<(int Year, int Month, decimal Amount)>> GetMonthlyAmountAsync();
    Task AddImportLogAsync(ImportLog log);
    Task<IReadOnlyList<ImportLog>> GetImportLogsAsync(ImportStatus? status);
}
