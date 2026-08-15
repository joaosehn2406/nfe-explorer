using Microsoft.EntityFrameworkCore;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;

namespace NfeExplorer_Api.Infrastructure.Data;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Invoices.AnyAsync())
        {
            return;
        }

        var now = DateTime.UtcNow;

        var issuers = new[]
        {
            new Issuer
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                LegalName = "Serra Azul Distribuidora LTDA",
                TradeName = "Serra Azul",
                CNPJ = "12457863000190",
                StateRegistration = "110042391114",
                Street = "Avenida das Industrias",
                Number = "1840",
                District = "Distrito Industrial",
                City = "Campinas",
                UF = "SP",
                ZipCode = "13035620"
            },
            new Issuer
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                LegalName = "Norte Sul Componentes S.A.",
                TradeName = "Norte Sul",
                CNPJ = "27619485000102",
                StateRegistration = "905331442188",
                Street = "Rua Itapura",
                Number = "731",
                District = "Centro",
                City = "Curitiba",
                UF = "PR",
                ZipCode = "80045120"
            },
            new Issuer
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                LegalName = "Atlas Papelaria Corporativa LTDA",
                TradeName = "Atlas Office",
                CNPJ = "43188270000155",
                StateRegistration = "224117093002",
                Street = "Rua dos Pinheiros",
                Number = "920",
                District = "Pinheiros",
                City = "Sao Paulo",
                UF = "SP",
                ZipCode = "05422001"
            },
            new Issuer
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                LegalName = "Lumen Laboratorios Integrados LTDA",
                TradeName = "Lumen Labs",
                CNPJ = "58904136000137",
                StateRegistration = "062184903114",
                Street = "Avenida Brasil",
                Number = "410",
                District = "Funcionarios",
                City = "Belo Horizonte",
                UF = "MG",
                ZipCode = "30140001"
            }
        };

        var recipients = new[]
        {
            new Recipient
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                LegalName = "Mercado Aurora LTDA",
                CNPJ = "15244071000166",
                StateRegistration = "149220784115",
                Street = "Rua Primavera",
                Number = "122",
                District = "Jardim Europa",
                City = "Sao Paulo",
                UF = "SP",
                ZipCode = "01449001"
            },
            new Recipient
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                LegalName = "Clinica Horizonte S.S.",
                CNPJ = "32740598000172",
                StateRegistration = "Isento",
                Street = "Rua Quinze de Novembro",
                Number = "640",
                District = "Centro",
                City = "Florianopolis",
                UF = "SC",
                ZipCode = "88010300"
            },
            new Recipient
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                LegalName = "Oficina Central de Equipamentos LTDA",
                CNPJ = "70954621000184",
                StateRegistration = "083117220094",
                Street = "Avenida Atlantica",
                Number = "2044",
                District = "Praia Comprida",
                City = "Santos",
                UF = "SP",
                ZipCode = "11060003"
            }
        };

        var carriers = new[]
        {
            new Carrier
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                LegalName = "Rota Clara Transportes LTDA",
                CNPJ = "96430281000109",
                StateRegistration = "411029300776",
                City = "Jundiai",
                UF = "SP",
                FreightMode = FreightMode.Sender
            },
            new Carrier
            {
                Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                LegalName = "Via Serra Logistica LTDA",
                CNPJ = "81367254000144",
                StateRegistration = "224099107885",
                City = "Joinville",
                UF = "SC",
                FreightMode = FreightMode.ThirdParty
            }
        };

        db.AddRange(issuers);
        db.AddRange(recipients);
        db.AddRange(carriers);

        var invoices = new[]
        {
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000001"),
                "35260312457863000190550010001284011054873018",
                now.AddMonths(-11),
                "Venda de mercadorias",
                "128401",
                "1",
                18420.90m,
                PaymentMethod.BankTransfer,
                InvoiceType.Outbound,
                issuers[0],
                recipients[0],
                carriers[0],
                18m,
                ("AZ-1100", "Cesta de alimentos premium", "19059090", 120m, 48.50m),
                ("AZ-2200", "Kit higiene institucional", "34012090", 90m, 74.45m),
                ("AZ-3300", "Reposicao de embalagens", "39232190", 240m, 24.60m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000002"),
                "35260427619485000102550010000977221099148273",
                now.AddMonths(-9).AddDays(4),
                "Compra para revenda",
                "097722",
                "1",
                42780.00m,
                PaymentMethod.InstantPayment,
                InvoiceType.Inbound,
                issuers[1],
                recipients[2],
                carriers[1],
                12m,
                ("NS-4100", "Modulo sensor industrial", "90318099", 36m, 690.00m),
                ("NS-4200", "Controlador de energia", "85371090", 18m, 760.00m),
                ("NS-4300", "Cabo blindado 5m", "85444200", 160m, 26.25m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000003"),
                "35260543188270000155550010000318491066420831",
                now.AddMonths(-8).AddDays(12),
                "Material de escritorio",
                "031849",
                "3",
                9180.35m,
                PaymentMethod.BankSlip,
                InvoiceType.Outbound,
                issuers[2],
                recipients[1],
                null,
                7m,
                ("AT-1001", "Papel sulfite A4 alcalino", "48025610", 320m, 18.90m),
                ("AT-1400", "Organizador de documentos", "39261000", 85m, 24.70m),
                ("AT-1720", "Caderno executivo capa dura", "48201000", 140m, 7.45m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000004"),
                "35260658904136000137550010000145231022900458",
                now.AddMonths(-7).AddDays(8),
                "Equipamentos laboratoriais",
                "014523",
                "1",
                67190.42m,
                PaymentMethod.CreditCard,
                InvoiceType.Outbound,
                issuers[3],
                recipients[1],
                carriers[1],
                18m,
                ("LU-7010", "Centrifuga compacta digital", "84211990", 4m, 11800.00m),
                ("LU-7020", "Micropipeta ajustavel", "90278099", 24m, 385.00m),
                ("LU-7030", "Reagente analitico lote B", "38220090", 180m, 59.39m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000005"),
                "35260712457863000190550010001299481074392066",
                now.AddMonths(-6).AddDays(10),
                "Venda de mercadorias",
                "129948",
                "1",
                23310.10m,
                PaymentMethod.BankTransfer,
                InvoiceType.Outbound,
                issuers[0],
                recipients[2],
                carriers[0],
                18m,
                ("AZ-5100", "Alimentos secos sortidos", "19041000", 210m, 52.90m),
                ("AZ-5200", "Bebida vegetal 1L", "22029900", 420m, 18.35m),
                ("AZ-5300", "Caixa retornavel padrao", "39231090", 75m, 59.50m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000006"),
                "35260827619485000102550010001002431050138927",
                now.AddMonths(-5).AddDays(15),
                "Componentes tecnicos",
                "100243",
                "1",
                58940.70m,
                PaymentMethod.InstantPayment,
                InvoiceType.Outbound,
                issuers[1],
                recipients[0],
                carriers[1],
                12m,
                ("NS-6100", "Fonte chaveada 24V", "85044090", 90m, 214.70m),
                ("NS-6200", "Interface ethernet industrial", "85176259", 44m, 680.00m),
                ("NS-6300", "Sensor optico reflexivo", "90314990", 120m, 82.95m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000007"),
                "35260943188270000155550010000351991014428935",
                now.AddMonths(-4).AddDays(2),
                "Suprimentos administrativos",
                "035199",
                "3",
                12480.00m,
                PaymentMethod.BankSlip,
                InvoiceType.Outbound,
                issuers[2],
                recipients[0],
                null,
                7m,
                ("AT-2100", "Envelope kraft documentacao", "48171000", 500m, 3.80m),
                ("AT-2300", "Toner corporativo preto", "84439933", 22m, 328.00m),
                ("AT-2500", "Etiqueta termica 100x150", "48219000", 80m, 42.10m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000008"),
                "35261058904136000137550010000160991093876204",
                now.AddMonths(-3).AddDays(18),
                "Insumos laboratoriais",
                "016099",
                "1",
                38120.55m,
                PaymentMethod.CreditCard,
                InvoiceType.Inbound,
                issuers[3],
                recipients[2],
                carriers[1],
                18m,
                ("LU-8110", "Frasco esteril 250ml", "39233000", 600m, 9.85m),
                ("LU-8120", "Filtro membrana 47mm", "84219999", 280m, 38.20m),
                ("LU-8130", "Padrao de calibracao", "38229000", 55m, 392.75m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000009"),
                "35261112457863000190550010001322041030774418",
                now.AddMonths(-2).AddDays(7),
                "Venda de mercadorias",
                "132204",
                "1",
                31670.80m,
                PaymentMethod.BankTransfer,
                InvoiceType.Outbound,
                issuers[0],
                recipients[1],
                carriers[0],
                18m,
                ("AZ-7100", "Linha de conservas premium", "20019000", 300m, 44.80m),
                ("AZ-7200", "Graos selecionados 5kg", "07133399", 260m, 59.10m),
                ("AZ-7300", "Display expositor modular", "94032000", 12m, 236.40m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000010"),
                "35261227619485000102550010001044801022066457",
                now.AddMonths(-1).AddDays(3),
                "Componentes tecnicos",
                "104480",
                "1",
                74450.20m,
                PaymentMethod.InstantPayment,
                InvoiceType.Outbound,
                issuers[1],
                recipients[2],
                carriers[1],
                12m,
                ("NS-9100", "Gateway de telemetria", "85176259", 28m, 1290.00m),
                ("NS-9200", "Modulo de seguranca I/O", "85389090", 36m, 745.50m),
                ("NS-9300", "Patch panel industrial", "85369090", 18m, 639.90m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000011"),
                "35261343188270000155550010000387451057261982",
                now.AddDays(-18),
                "Material de escritorio",
                "038745",
                "3",
                14890.60m,
                PaymentMethod.BankSlip,
                InvoiceType.Inbound,
                issuers[2],
                recipients[1],
                null,
                7m,
                ("AT-3100", "Arquivo morto reforcado", "48191000", 260m, 8.20m),
                ("AT-3200", "Caneta gel preta", "96081000", 720m, 3.40m),
                ("AT-3300", "Suporte monitor metalico", "73269090", 45m, 228.50m)),
            CreateInvoice(
                Guid.Parse("10000000-0000-0000-0000-000000000012"),
                "35261458904136000137550010000177821043502001",
                now.AddDays(-5),
                "Equipamentos laboratoriais",
                "017782",
                "1",
                92340.00m,
                PaymentMethod.BankTransfer,
                InvoiceType.Outbound,
                issuers[3],
                recipients[0],
                carriers[1],
                18m,
                ("LU-9010", "Espectrofotometro UV-Vis", "90273020", 2m, 28600.00m),
                ("LU-9020", "Cabine de seguranca biologica", "84148090", 1m, 24800.00m),
                ("LU-9030", "Contrato de instalacao tecnica", "99871900", 1m, 10340.00m))
        };

        db.Invoices.AddRange(invoices);

        db.ImportLogs.AddRange(
            invoices.Select(invoice => new ImportLog
            {
                Id = Guid.NewGuid(),
                Timestamp = invoice.ImportedAt,
                Status = ImportStatus.Success,
                FileName = $"nfe-{invoice.InvoiceNumber}.xml",
                InvoiceNumber = invoice.InvoiceNumber,
                Issuer = invoice.Issuer.LegalName,
                Amount = invoice.TotalAmount,
                Message = "Imported successfully."
            }));

        db.ImportLogs.AddRange(
            new ImportLog
            {
                Id = Guid.NewGuid(),
                Timestamp = now.AddDays(-3),
                Status = ImportStatus.Duplicate,
                FileName = "nfe-104480-reenvio.xml",
                InvoiceNumber = "104480",
                Issuer = "Norte Sul Componentes S.A.",
                Amount = 74450.20m,
                Message = "Invoice #104480 has already been imported."
            },
            new ImportLog
            {
                Id = Guid.NewGuid(),
                Timestamp = now.AddDays(-1).AddHours(-2),
                Status = ImportStatus.Error,
                FileName = "fornecedor-sem-infNFe.xml",
                Message = "XML does not contain a valid NF-e."
            });

        await db.SaveChangesAsync();
    }

    private static Invoice CreateInvoice(
        Guid id,
        string accessKey,
        DateTime issuedAt,
        string operationNature,
        string invoiceNumber,
        string series,
        decimal totalAmount,
        PaymentMethod paymentMethod,
        InvoiceType invoiceType,
        Issuer issuer,
        Recipient recipient,
        Carrier? carrier,
        decimal icmsRate,
        params (string Code, string Description, string Ncm, decimal Quantity, decimal UnitAmount)[] products)
    {
        var productEntities = products.Select(product => new Product
        {
            Id = Guid.NewGuid(),
            ProductCode = product.Code,
            Description = product.Description,
            NCM = product.Ncm,
            Quantity = product.Quantity,
            UnitAmount = product.UnitAmount,
            TotalAmount = Math.Round(product.Quantity * product.UnitAmount, 2)
        }).ToList();

        var productAmount = productEntities.Sum(product => product.TotalAmount);
        var icmsAmount = Math.Round(productAmount * icmsRate / 100m, 2);
        var pisAmount = Math.Round(productAmount * 0.0165m, 2);
        var cofinsAmount = Math.Round(productAmount * 0.076m, 2);

        var invoice = new Invoice
        {
            Id = id,
            AccessKey = accessKey,
            IssuedAt = DateTime.SpecifyKind(issuedAt, DateTimeKind.Utc),
            ImportedAt = DateTime.SpecifyKind(issuedAt.AddHours(3), DateTimeKind.Utc),
            OperationNature = operationNature,
            InvoiceNumber = invoiceNumber,
            Series = series,
            TotalAmount = totalAmount,
            PaidAmount = totalAmount,
            PaymentMethod = paymentMethod,
            InvoiceType = invoiceType,
            Issuer = issuer,
            IssuerId = issuer.Id,
            Recipient = recipient,
            RecipientId = recipient.Id,
            Carrier = carrier,
            CarrierId = carrier?.Id,
            Products = productEntities
        };

        invoice.Taxes = new NfeTaxes
        {
            Id = Guid.NewGuid(),
            Invoice = invoice,
            InvoiceId = invoice.Id,
            ProductAmount = productAmount,
            IcmsTaxBase = productAmount,
            IcmsAmount = icmsAmount,
            PisAmount = pisAmount,
            CofinsAmount = cofinsAmount,
            TotalTaxesAmount = icmsAmount + pisAmount + cofinsAmount,
            InvoiceAmount = totalAmount,
            IcmsRate = icmsRate
        };

        return invoice;
    }
}
