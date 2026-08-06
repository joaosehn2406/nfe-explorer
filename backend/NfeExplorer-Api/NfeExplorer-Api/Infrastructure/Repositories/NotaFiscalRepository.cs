using Microsoft.EntityFrameworkCore;
using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;
using NfeExplorer_Api.Domain.Interfaces;
using NfeExplorer_Api.Infrastructure.Data;

namespace NfeExplorer_Api.Infrastructure.Repositories;

public class NotaFiscalRepository : INotaFiscalRepository
{
    private readonly AppDbContext _context;

    public NotaFiscalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<NotaFiscal?> GetByIdAsync(Guid id)
    {
        return await _context.NotaFiscais
            .Include(nota => nota.Emitente)
            .Include(nota => nota.Destinatario)
            .Include(nota => nota.Transportadora)
            .Include(nota => nota.Produtos)
            .Include(nota => nota.ImpostosNfe)
            .FirstOrDefaultAsync(nota => nota.Id == id);
    }

    public async Task<NotaFiscal?> GetByChaveAsync(string chave)
    {
        return await _context.NotaFiscais
            .Include(nota => nota.Emitente)
            .Include(nota => nota.Destinatario)
            .Include(nota => nota.Transportadora)
            .Include(nota => nota.Produtos)
            .Include(nota => nota.ImpostosNfe)
            .FirstOrDefaultAsync(nota => nota.ChaveAcesso == chave);
    }

    public async Task AddAsync(NotaFiscal notaFiscal)
    {
        await _context.NotaFiscais.AddAsync(notaFiscal);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var nota = await _context.NotaFiscais.FirstOrDefaultAsync(nota => nota.Id == id);

        if (nota == null)
            return false;

        _context.NotaFiscais.Remove(nota);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(IReadOnlyList<NotaFiscal> Items, int Total)> GetAllAsync(NfeListRequest filter)
    {
        var query = _context.NotaFiscais
            .Include(nota => nota.Emitente)
            .Include(nota => nota.Destinatario)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var termo = filter.Search.Trim().ToLower();
            query = query.Where(nota =>
                nota.NumeroNota.ToLower().Contains(termo) ||
                nota.ChaveAcesso.ToLower().Contains(termo) ||
                nota.Emitente.RazaoSocial.ToLower().Contains(termo) ||
                (nota.Emitente.NomeFantasia != null && nota.Emitente.NomeFantasia.ToLower().Contains(termo)) ||
                nota.Destinatario.RazaoSocial.ToLower().Contains(termo));
        }

        if (filter.Tipo.HasValue)
            query = query.Where(nota => nota.TipoNota == filter.Tipo.Value);

        if (!string.IsNullOrWhiteSpace(filter.Emitente))
            query = query.Where(nota =>
                nota.Emitente.NomeFantasia == filter.Emitente ||
                nota.Emitente.RazaoSocial == filter.Emitente);

        if (filter.DataDe.HasValue)
            query = query.Where(nota => nota.DataEmissao >= filter.DataDe.Value);

        if (filter.DataAte.HasValue)
            query = query.Where(nota => nota.DataEmissao <= filter.DataAte.Value);

        var total = await query.CountAsync();

        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;

        var items = await query
            .OrderByDescending(nota => nota.DataEmissao)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<IReadOnlyList<string>> GetEmitentesAsync()
    {
        return await _context.NotaFiscais
            .Select(nota => nota.Emitente.NomeFantasia ?? nota.Emitente.RazaoSocial)
            .Distinct()
            .OrderBy(nome => nome)
            .ToListAsync();
    }

    public Task<int> CountAsync() => _context.NotaFiscais.CountAsync();

    public async Task<decimal> SumValorTotalAsync()
    {
        return await _context.NotaFiscais.AnyAsync()
            ? await _context.NotaFiscais.SumAsync(nota => nota.ValorTotal)
            : 0m;
    }

    public Task<int> CountByTipoAsync(TipoNota tipo) =>
        _context.NotaFiscais.CountAsync(nota => nota.TipoNota == tipo);

    public async Task<IReadOnlyList<(string Nome, decimal Valor)>> GetTopEmitentesAsync(int take)
    {
        var resultado = await _context.NotaFiscais
            .GroupBy(nota => nota.Emitente.NomeFantasia ?? nota.Emitente.RazaoSocial)
            .Select(grupo => new { Nome = grupo.Key, Valor = grupo.Sum(nota => nota.ValorTotal) })
            .OrderByDescending(item => item.Valor)
            .Take(take)
            .ToListAsync();

        return resultado.Select(item => (item.Nome, item.Valor)).ToList();
    }

    public async Task<IReadOnlyList<(int Ano, int Mes, decimal Valor)>> GetValorPorMesAsync()
    {
        var resultado = await _context.NotaFiscais
            .GroupBy(nota => new { nota.DataEmissao.Year, nota.DataEmissao.Month })
            .Select(grupo => new
            {
                grupo.Key.Year,
                grupo.Key.Month,
                Valor = grupo.Sum(nota => nota.ValorTotal)
            })
            .OrderBy(item => item.Year).ThenBy(item => item.Month)
            .ToListAsync();

        return resultado.Select(item => (item.Year, item.Month, item.Valor)).ToList();
    }

    public async Task AddImportLogAsync(ImportLog log)
    {
        await _context.ImportLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<ImportLog>> GetImportLogsAsync(StatusImportacao? status)
    {
        var query = _context.ImportLogs.AsQueryable();

        if (status.HasValue)
            query = query.Where(log => log.Status == status.Value);

        return await query
            .OrderByDescending(log => log.DataHora)
            .Take(200)
            .ToListAsync();
    }
}
