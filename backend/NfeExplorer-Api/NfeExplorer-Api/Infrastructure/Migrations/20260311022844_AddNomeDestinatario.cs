using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NfeExplorer_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddNomeDestinatario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomePessoa",
                table: "Destinatarios",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomePessoa",
                table: "Destinatarios");
        }
    }
}
