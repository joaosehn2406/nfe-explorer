using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Application.Mappings;

public static class ImportLogMapper
{
    public static ImportLog Success(string fileName, Invoice invoice)
    {
        return new ImportLog
        {
            Timestamp = DateTime.UtcNow,
            Status = ImportStatus.Success,
            FileName = fileName,
            InvoiceNumber = invoice.InvoiceNumber,
            Issuer = invoice.Issuer.TradeName ?? invoice.Issuer.LegalName,
            Amount = invoice.TotalAmount,
            Message = "Imported successfully."
        };
    }

    public static ImportLog Duplicate(string fileName, Invoice? invoice, string message)
    {
        return new ImportLog
        {
            Timestamp = DateTime.UtcNow,
            Status = ImportStatus.Duplicate,
            FileName = fileName,
            InvoiceNumber = invoice?.InvoiceNumber,
            Issuer = invoice is null ? null : invoice.Issuer.TradeName ?? invoice.Issuer.LegalName,
            Amount = invoice?.TotalAmount,
            Message = message
        };
    }

    public static ImportLog Error(string fileName, string message)
    {
        return new ImportLog
        {
            Timestamp = DateTime.UtcNow,
            Status = ImportStatus.Error,
            FileName = fileName,
            Message = message
        };
    }

    public static ImportLogResponse ToResponse(ImportLog log)
    {
        return new ImportLogResponse
        {
            Id = log.Id,
            Timestamp = log.Timestamp,
            Status = log.Status,
            FileName = log.FileName,
            InvoiceNumber = log.InvoiceNumber,
            Issuer = log.Issuer,
            Amount = log.Amount,
            Message = log.Message
        };
    }
}
