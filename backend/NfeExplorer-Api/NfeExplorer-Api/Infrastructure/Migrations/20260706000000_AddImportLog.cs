using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NfeExplorer_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddImportLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    NomeArquivo = table.Column<string>(type: "text", nullable: false),
                    NumeroNota = table.Column<string>(type: "text", nullable: true),
                    Emitente = table.Column<string>(type: "text", nullable: true),
                    Valor = table.Column<decimal>(type: "numeric", nullable: true),
                    Mensagem = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportLogs");
        }
    }
}
