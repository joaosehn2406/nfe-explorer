using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NfeExplorer_Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Destinatarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RazaoSocial = table.Column<string>(type: "text", nullable: false),
                    CNPJ = table.Column<string>(type: "text", nullable: true),
                    CPF = table.Column<string>(type: "text", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "text", nullable: true),
                    Logradouro = table.Column<string>(type: "text", nullable: false),
                    Numero = table.Column<string>(type: "text", nullable: false),
                    Bairro = table.Column<string>(type: "text", nullable: false),
                    Municipio = table.Column<string>(type: "text", nullable: false),
                    UF = table.Column<string>(type: "text", nullable: false),
                    CEP = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinatarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emitentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RazaoSocial = table.Column<string>(type: "text", nullable: false),
                    NomeFantasia = table.Column<string>(type: "text", nullable: true),
                    CNPJ = table.Column<string>(type: "text", nullable: false),
                    InscricaoEstadual = table.Column<string>(type: "text", nullable: true),
                    Logradouro = table.Column<string>(type: "text", nullable: false),
                    Numero = table.Column<string>(type: "text", nullable: false),
                    Bairro = table.Column<string>(type: "text", nullable: false),
                    Municipio = table.Column<string>(type: "text", nullable: false),
                    UF = table.Column<string>(type: "text", nullable: false),
                    CEP = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emitentes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transportadoras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RazaoSocial = table.Column<string>(type: "text", nullable: false),
                    CNPJ = table.Column<string>(type: "text", nullable: true),
                    CPF = table.Column<string>(type: "text", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "text", nullable: true),
                    Municipio = table.Column<string>(type: "text", nullable: true),
                    UF = table.Column<string>(type: "text", nullable: true),
                    ModalidadeFrete = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportadoras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotaFiscais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChaveAcesso = table.Column<string>(type: "text", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataImportacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NaturezaOperacao = table.Column<string>(type: "text", nullable: false),
                    NumeroNota = table.Column<string>(type: "text", nullable: false),
                    Serie = table.Column<string>(type: "text", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorPago = table.Column<decimal>(type: "numeric", nullable: false),
                    FormaPagamento = table.Column<int>(type: "integer", nullable: false),
                    TipoNota = table.Column<int>(type: "integer", nullable: false),
                    IdEmitente = table.Column<Guid>(type: "uuid", nullable: false),
                    IdDestinatario = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTransportadora = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaFiscais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotaFiscais_Destinatarios_IdDestinatario",
                        column: x => x.IdDestinatario,
                        principalTable: "Destinatarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotaFiscais_Emitentes_IdEmitente",
                        column: x => x.IdEmitente,
                        principalTable: "Emitentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotaFiscais_Transportadoras_IdTransportadora",
                        column: x => x.IdTransportadora,
                        principalTable: "Transportadoras",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImpostosNfes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdNotaFiscal = table.Column<Guid>(type: "uuid", nullable: false),
                    ValorProdutos = table.Column<decimal>(type: "numeric", nullable: false),
                    BaseCalculoICMS = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorICMS = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorPIS = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorCOFINS = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorTotalTributos = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorNota = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpostosNfes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImpostosNfes_NotaFiscais_IdNotaFiscal",
                        column: x => x.IdNotaFiscal,
                        principalTable: "NotaFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoProduto = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    NCM = table.Column<string>(type: "text", nullable: false),
                    CFOP = table.Column<string>(type: "text", nullable: false),
                    Unidade = table.Column<string>(type: "text", nullable: false),
                    Quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    IdNotaFiscal = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_NotaFiscais_IdNotaFiscal",
                        column: x => x.IdNotaFiscal,
                        principalTable: "NotaFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImpostosNfes_IdNotaFiscal",
                table: "ImpostosNfes",
                column: "IdNotaFiscal",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotaFiscais_IdDestinatario",
                table: "NotaFiscais",
                column: "IdDestinatario");

            migrationBuilder.CreateIndex(
                name: "IX_NotaFiscais_IdEmitente",
                table: "NotaFiscais",
                column: "IdEmitente");

            migrationBuilder.CreateIndex(
                name: "IX_NotaFiscais_IdTransportadora",
                table: "NotaFiscais",
                column: "IdTransportadora");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_IdNotaFiscal",
                table: "Produtos",
                column: "IdNotaFiscal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImpostosNfes");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "NotaFiscais");

            migrationBuilder.DropTable(
                name: "Destinatarios");

            migrationBuilder.DropTable(
                name: "Emitentes");

            migrationBuilder.DropTable(
                name: "Transportadoras");
        }
    }
}
