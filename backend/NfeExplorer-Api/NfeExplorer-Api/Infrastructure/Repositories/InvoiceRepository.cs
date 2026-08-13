using Microsoft.EntityFrameworkCore;
using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;
using NfeExplorer_Api.Domain.Interfaces;
using NfeExplorer_Api.Infrastructure.Data;

namespace NfeExplorer_Api.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice?> GetByIdAsync(Guid id)
    {
        return await _context.Invoices
            .Include(invoice => invoice.Issuer)
            .Include(invoice => invoice.Recipient)
            .Include(invoice => invoice.Carrier)
            .Include(invoice => invoice.Products)
            .Include(invoice => invoice.Taxes)
            .FirstOrDefaultAsync(invoice => invoice.Id == id);
    }

    public async Task<Invoice?> GetByAccessKeyAsync(string accessKey)
    {
        return await _context.Invoices
            .Include(invoice => invoice.Issuer)
            .Include(invoice => invoice.Recipient)
            .Include(invoice => invoice.Carrier)
            .Include(invoice => invoice.Products)
            .Include(invoice => invoice.Taxes)
            .FirstOrDefaultAsync(invoice => invoice.AccessKey == accessKey);
    }

    public async Task AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var invoice = await _context.Invoices.FirstOrDefaultAsync(invoice => invoice.Id == id);

        if (invoice == null)
        {
            return false;
        }

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(IReadOnlyList<Invoice> Items, int Total)> GetAllAsync(NfeListRequest filter)
    {
        var query = _context.Invoices
            .Include(invoice => invoice.Issuer)
            .Include(invoice => invoice.Recipient)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim().ToLower();
            query = query.Where(invoice =>
                invoice.InvoiceNumber.ToLower().Contains(term) ||
                invoice.AccessKey.ToLower().Contains(term) ||
                invoice.Issuer.LegalName.ToLower().Contains(term) ||
                (invoice.Issuer.TradeName != null && invoice.Issuer.TradeName.ToLower().Contains(term)) ||
                invoice.Recipient.LegalName.ToLower().Contains(term));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(invoice => invoice.InvoiceType == filter.Type.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Issuer))
        {
            query = query.Where(invoice =>
                invoice.Issuer.TradeName == filter.Issuer ||
                invoice.Issuer.LegalName == filter.Issuer);
        }

        if (filter.IssuedFrom.HasValue)
        {
            query = query.Where(invoice => invoice.IssuedAt >= filter.IssuedFrom.Value);
        }

        if (filter.IssuedTo.HasValue)
        {
            query = query.Where(invoice => invoice.IssuedAt <= filter.IssuedTo.Value);
        }

        var total = await query.CountAsync();

        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;

        var items = await query
            .OrderByDescending(invoice => invoice.IssuedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<IReadOnlyList<string>> GetIssuersAsync()
    {
        return await _context.Invoices
            .Select(invoice => invoice.Issuer.TradeName ?? invoice.Issuer.LegalName)
            .Distinct()
            .OrderBy(name => name)
            .ToListAsync();
    }

    public Task<int> CountAsync() => _context.Invoices.CountAsync();

    public async Task<decimal> SumTotalAmountAsync()
    {
        return await _context.Invoices.AnyAsync()
            ? await _context.Invoices.SumAsync(invoice => invoice.TotalAmount)
            : 0m;
    }

    public Task<int> CountByTypeAsync(InvoiceType type) =>
        _context.Invoices.CountAsync(invoice => invoice.InvoiceType == type);

    public async Task<IReadOnlyList<(string Name, decimal Amount)>> GetTopIssuersAsync(int take)
    {
        var result = await _context.Invoices
            .GroupBy(invoice => invoice.Issuer.TradeName ?? invoice.Issuer.LegalName)
            .Select(group => new { Name = group.Key, Amount = group.Sum(invoice => invoice.TotalAmount) })
            .OrderByDescending(item => item.Amount)
            .Take(take)
            .ToListAsync();

        return result.Select(item => (item.Name, item.Amount)).ToList();
    }

    public async Task<IReadOnlyList<(int Year, int Month, decimal Amount)>> GetMonthlyAmountAsync()
    {
        var result = await _context.Invoices
            .GroupBy(invoice => new { invoice.IssuedAt.Year, invoice.IssuedAt.Month })
            .Select(group => new
            {
                group.Key.Year,
                group.Key.Month,
                Amount = group.Sum(invoice => invoice.TotalAmount)
            })
            .OrderBy(item => item.Year)
            .ThenBy(item => item.Month)
            .ToListAsync();

        return result.Select(item => (item.Year, item.Month, item.Amount)).ToList();
    }

    public async Task AddImportLogAsync(ImportLog log)
    {
        await _context.ImportLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<ImportLog>> GetImportLogsAsync(ImportStatus? status)
    {
        var query = _context.ImportLogs.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(log => log.Status == status.Value);
        }

        return await query
            .OrderByDescending(log => log.Timestamp)
            .Take(200)
            .ToListAsync();
    }
}
