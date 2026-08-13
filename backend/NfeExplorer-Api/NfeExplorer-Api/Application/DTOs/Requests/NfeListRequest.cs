using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.DTOs.Requests;

public class NfeListRequest
{
    public string? Search { get; set; }
    public InvoiceType? Type { get; set; }
    public string? Issuer { get; set; }
    public DateTime? IssuedFrom { get; set; }
    public DateTime? IssuedTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
