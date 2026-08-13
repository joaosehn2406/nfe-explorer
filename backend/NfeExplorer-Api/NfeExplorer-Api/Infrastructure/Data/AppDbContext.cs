using Microsoft.EntityFrameworkCore;
using NfeExplorer_Api.Domain.Entities;

namespace NfeExplorer_Api.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Recipient> Recipients { get; set; }
    public DbSet<Issuer> Issuers { get; set; }
    public DbSet<NfeTaxes> NfeTaxes { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Carrier> Carriers { get; set; }
    public DbSet<ImportLog> ImportLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureInvoice(modelBuilder);
        ConfigureIssuer(modelBuilder);
        ConfigureRecipient(modelBuilder);
        ConfigureProduct(modelBuilder);
        ConfigureCarrier(modelBuilder);
        ConfigureTaxes(modelBuilder);
        ConfigureImportLog(modelBuilder);
    }

    private static void ConfigureInvoice(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Invoice>();
        entity.ToTable("NotaFiscais");
        entity.Property(invoice => invoice.AccessKey).HasColumnName("ChaveAcesso");
        entity.Property(invoice => invoice.IssuedAt).HasColumnName("DataEmissao");
        entity.Property(invoice => invoice.ImportedAt).HasColumnName("DataImportacao");
        entity.Property(invoice => invoice.OperationNature).HasColumnName("NaturezaOperacao");
        entity.Property(invoice => invoice.InvoiceNumber).HasColumnName("NumeroNota");
        entity.Property(invoice => invoice.Series).HasColumnName("Serie");
        entity.Property(invoice => invoice.TotalAmount).HasColumnName("ValorTotal");
        entity.Property(invoice => invoice.PaidAmount).HasColumnName("ValorPago");
        entity.Property(invoice => invoice.PaymentMethod).HasColumnName("FormaPagamento");
        entity.Property(invoice => invoice.InvoiceType).HasColumnName("TipoNota");
        entity.Property(invoice => invoice.IssuerId).HasColumnName("IdEmitente");
        entity.Property(invoice => invoice.RecipientId).HasColumnName("IdDestinatario");
        entity.Property(invoice => invoice.CarrierId).HasColumnName("IdTransportadora");

        entity.HasOne(invoice => invoice.Issuer)
            .WithMany(issuer => issuer.Invoices)
            .HasForeignKey(invoice => invoice.IssuerId);

        entity.HasOne(invoice => invoice.Recipient)
            .WithMany(recipient => recipient.Invoices)
            .HasForeignKey(invoice => invoice.RecipientId);

        entity.HasOne(invoice => invoice.Carrier)
            .WithMany(carrier => carrier.Invoices)
            .HasForeignKey(invoice => invoice.CarrierId);
    }

    private static void ConfigureIssuer(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Issuer>();
        entity.ToTable("Emitentes");
        entity.Property(issuer => issuer.LegalName).HasColumnName("RazaoSocial");
        entity.Property(issuer => issuer.TradeName).HasColumnName("NomeFantasia");
        entity.Property(issuer => issuer.StateRegistration).HasColumnName("InscricaoEstadual");
        entity.Property(issuer => issuer.Street).HasColumnName("Logradouro");
        entity.Property(issuer => issuer.Number).HasColumnName("Numero");
        entity.Property(issuer => issuer.District).HasColumnName("Bairro");
        entity.Property(issuer => issuer.City).HasColumnName("Municipio");
        entity.Property(issuer => issuer.ZipCode).HasColumnName("CEP");
    }

    private static void ConfigureRecipient(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Recipient>();
        entity.ToTable("Destinatarios");
        entity.Property(recipient => recipient.LegalName).HasColumnName("RazaoSocial");
        entity.Property(recipient => recipient.StateRegistration).HasColumnName("InscricaoEstadual");
        entity.Property(recipient => recipient.Street).HasColumnName("Logradouro");
        entity.Property(recipient => recipient.Number).HasColumnName("Numero");
        entity.Property(recipient => recipient.District).HasColumnName("Bairro");
        entity.Property(recipient => recipient.City).HasColumnName("Municipio");
        entity.Property(recipient => recipient.PersonName).HasColumnName("NomePessoa");
        entity.Property(recipient => recipient.ZipCode).HasColumnName("CEP");
    }

    private static void ConfigureProduct(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Product>();
        entity.ToTable("Produtos");
        entity.Property(product => product.ProductCode).HasColumnName("CodigoProduto");
        entity.Property(product => product.Description).HasColumnName("Descricao");
        entity.Property(product => product.Quantity).HasColumnName("Quantidade");
        entity.Property(product => product.UnitAmount).HasColumnName("ValorUnitario");
        entity.Property(product => product.TotalAmount).HasColumnName("ValorTotal");
        entity.Property(product => product.InvoiceId).HasColumnName("IdNotaFiscal");

        entity.HasOne(product => product.Invoice)
            .WithMany(invoice => invoice.Products)
            .HasForeignKey(product => product.InvoiceId);
    }

    private static void ConfigureCarrier(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Carrier>();
        entity.ToTable("Transportadoras");
        entity.Property(carrier => carrier.LegalName).HasColumnName("RazaoSocial");
        entity.Property(carrier => carrier.StateRegistration).HasColumnName("InscricaoEstadual");
        entity.Property(carrier => carrier.City).HasColumnName("Municipio");
        entity.Property(carrier => carrier.FreightMode).HasColumnName("ModalidadeFrete");
    }

    private static void ConfigureTaxes(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<NfeTaxes>();
        entity.ToTable("ImpostosNfes");
        entity.Property(taxes => taxes.InvoiceId).HasColumnName("IdNotaFiscal");
        entity.Property(taxes => taxes.ProductAmount).HasColumnName("ValorProdutos");
        entity.Property(taxes => taxes.IcmsTaxBase).HasColumnName("BaseCalculoICMS");
        entity.Property(taxes => taxes.IcmsAmount).HasColumnName("ValorICMS");
        entity.Property(taxes => taxes.PisAmount).HasColumnName("ValorPIS");
        entity.Property(taxes => taxes.CofinsAmount).HasColumnName("ValorCOFINS");
        entity.Property(taxes => taxes.TotalTaxesAmount).HasColumnName("ValorTotalTributos");
        entity.Property(taxes => taxes.InvoiceAmount).HasColumnName("ValorNota");
        entity.Property(taxes => taxes.IcmsRate).HasColumnName("AliquotaIcms");

        entity.HasOne(taxes => taxes.Invoice)
            .WithOne(invoice => invoice.Taxes)
            .HasForeignKey<NfeTaxes>(taxes => taxes.InvoiceId);
    }

    private static void ConfigureImportLog(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ImportLog>();
        entity.Property(log => log.FileName).HasColumnName("NomeArquivo");
        entity.Property(log => log.InvoiceNumber).HasColumnName("NumeroNota");
        entity.Property(log => log.Issuer).HasColumnName("Emitente");
        entity.Property(log => log.Amount).HasColumnName("Valor");
        entity.Property(log => log.Message).HasColumnName("Mensagem");
        entity.Property(log => log.Timestamp).HasColumnName("DataHora");
    }
}
