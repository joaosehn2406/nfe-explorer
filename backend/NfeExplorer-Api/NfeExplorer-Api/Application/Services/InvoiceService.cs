using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Application.Exception;
using NfeExplorer_Api.Application.Interfaces;
using NfeExplorer_Api.Application.Mappings;
using NfeExplorer_Api.Application.Validators;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;
using NfeExplorer_Api.Domain.Interfaces;
using NfeExplorer_Api.Infrastructure.Parsers;

namespace NfeExplorer_Api.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _repository;

    public InvoiceService(IInvoiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<ImportNfeResponse> AddAsync(ParseNfeRequest request)
    {
        var fileName = request?.File?.FileName ?? "Pasted XML";
        Invoice? invoice = null;

        try
        {
            NFeValidator.ValidateRequest(request!);

            string xml;
            if (request!.File != null)
            {
                using var stream = request.File.OpenReadStream();
                using var reader = new StreamReader(stream);
                xml = await reader.ReadToEndAsync();
            }
            else
            {
                xml = request.XmlText!;
            }

            NFeValidator.ValidateXml(xml);

            invoice = NfeParser.Parse(xml);

            if (invoice.AccessKey.Length > 44)
            {
                throw new ArgumentException("Invoice access key exceeds 44 digits.");
            }

            var existingInvoice = await _repository.GetByAccessKeyAsync(invoice.AccessKey);
            if (existingInvoice != null)
            {
                throw new DuplicateNfeException($"Invoice #{invoice.InvoiceNumber} has already been imported.");
            }

            await _repository.AddAsync(invoice);
            await _repository.AddImportLogAsync(ImportLogMapper.Success(fileName, invoice));

            return ImportNfeMapper.ToImportNfeResponse(invoice);
        }
        catch (DuplicateNfeException ex)
        {
            await _repository.AddImportLogAsync(ImportLogMapper.Duplicate(fileName, invoice, ex.Message));
            throw;
        }
        catch (ArgumentException ex)
        {
            await _repository.AddImportLogAsync(ImportLogMapper.Error(fileName, ex.Message));
            throw;
        }
    }

    public async Task<NfeDetailsResponse?> GetByIdAsync(Guid id)
    {
        var invoice = await _repository.GetByIdAsync(id);
        return invoice == null ? null : NfeDetailsMapper.ToResponse(invoice);
    }

    public async Task<NfeDetailsResponse?> GetByAccessKeyAsync(string accessKey)
    {
        var invoice = await _repository.GetByAccessKeyAsync(accessKey);
        return invoice == null ? null : NfeDetailsMapper.ToResponse(invoice);
    }

    public Task<bool> DeleteAsync(Guid id) => _repository.DeleteAsync(id);

    public async Task<PagedResponse<NfeListItemResponse>> GetAllAsync(NfeListRequest filter)
    {
        var (items, total) = await _repository.GetAllAsync(filter);

        return new PagedResponse<NfeListItemResponse>
        {
            Items = items.Select(NfeListMapper.ToListItem).ToList(),
            Total = total,
            Page = filter.Page < 1 ? 1 : filter.Page,
            PageSize = filter.PageSize < 1 ? 10 : filter.PageSize
        };
    }

    public Task<IReadOnlyList<string>> GetIssuersAsync() => _repository.GetIssuersAsync();

    public async Task<DashboardStatsResponse> GetDashboardAsync()
    {
        var topIssuers = await _repository.GetTopIssuersAsync(5);
        var monthlyAmounts = await _repository.GetMonthlyAmountAsync();

        return new DashboardStatsResponse
        {
            TotalInvoices = await _repository.CountAsync(),
            TotalAmount = await _repository.SumTotalAmountAsync(),
            TotalOutbound = await _repository.CountByTypeAsync(InvoiceType.Outbound),
            TotalInbound = await _repository.CountByTypeAsync(InvoiceType.Inbound),
            TopIssuers = topIssuers
                .Select(item => new TopIssuerResponse { Name = item.Name, Amount = item.Amount })
                .ToList(),
            MonthlyInvoices = monthlyAmounts
                .Select(item => new MonthlyInvoicesResponse { Year = item.Year, Month = item.Month, Amount = item.Amount })
                .ToList()
        };
    }

    public async Task<IReadOnlyList<ImportLogResponse>> GetHistoryAsync(ImportStatus? status)
    {
        var logs = await _repository.GetImportLogsAsync(status);
        return logs.Select(ImportLogMapper.ToResponse).ToList();
    }
}
