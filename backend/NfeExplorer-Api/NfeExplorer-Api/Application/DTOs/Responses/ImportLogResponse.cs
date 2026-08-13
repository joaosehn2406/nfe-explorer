using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.DTOs.Responses;

public class ImportLogResponse
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public ImportStatus Status { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string? InvoiceNumber { get; set; }
    public string? Issuer { get; set; }
    public decimal? Amount { get; set; }
    public string? Message { get; set; }
}
