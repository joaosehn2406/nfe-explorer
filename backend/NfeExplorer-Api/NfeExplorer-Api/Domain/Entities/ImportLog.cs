using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Domain.Entities;

public class ImportLog
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public required ImportStatus Status { get; set; }
    public required string FileName { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? Issuer { get; set; }
    public decimal? Amount { get; set; }
    public required string Message { get; set; }
}
