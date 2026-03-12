using Microsoft.EntityFrameworkCore;
using NfeExplorer_Api.Domain.Entities;

namespace NfeExplorer_Api.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Destinatario> Destinatarios { get; set; }
    public DbSet<Emitente> Emitentes { get; set; }
    public DbSet<ImpostosNfe> ImpostosNfes { get; set; }
    public DbSet<NotaFiscal> NotaFiscais { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Transportadora> Transportadoras { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotaFiscal>()
            .HasOne(nota => nota.Emitente)
            .WithMany(emitente => emitente.NotasFiscais)
            .HasForeignKey(nota => nota.IdEmitente);

        modelBuilder.Entity<NotaFiscal>()
            .HasOne(nota => nota.Destinatario)
            .WithMany(destinatario => destinatario.NotasFiscais)
            .HasForeignKey(nota => nota.IdDestinatario);

        modelBuilder.Entity<NotaFiscal>()
            .HasOne(nota => nota.Transportadora)
            .WithMany(transpo => transpo.NotasFiscais)
            .HasForeignKey(nota => nota.IdTransportadora);

        modelBuilder.Entity<Produto>()
            .HasOne(prod => prod.NotaFiscal)
            .WithMany(nota => nota.Produtos)
            .HasForeignKey(prod => prod.IdNotaFiscal);

        modelBuilder.Entity<ImpostosNfe>()
            .HasOne(imposto => imposto.NotaFiscal)
            .WithOne(nota => nota.ImpostosNfe)
            .HasForeignKey<ImpostosNfe>(imposto => imposto.IdNotaFiscal);
    }
}