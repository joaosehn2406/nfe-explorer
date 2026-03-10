using Microsoft.EntityFrameworkCore;
using NfeExplorer_Api.Data;
using NfeExplorer_Api.Models;

namespace NfeExplorer_Api.Repository;

public class NotaFiscalRepository : INotaFiscalRepository
{
    private readonly AppDbContext _context;

    public NotaFiscalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<NotaFiscal?> GetByIdAsync(Guid id)
    {
        return await _context.NotaFiscais.FindAsync(id);
    }

    public async Task<NotaFiscal?> GetByChaveAsync(string chave)
    {
        return await _context.NotaFiscais
            .FirstOrDefaultAsync(nota => nota.ChaveAcesso == chave);
    }

    public async Task AddAsync(NotaFiscal notaFiscal)
    {
        await _context.NotaFiscais.AddAsync(notaFiscal);
        await _context.SaveChangesAsync();
    }
}