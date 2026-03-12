using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NfeExplorer_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCampoAliquotaIcmsERemovidoCampoCFOP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CFOP",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Unidade",
                table: "Produtos");

            migrationBuilder.AddColumn<decimal>(
                name: "AliquotaIcms",
                table: "ImpostosNfes",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AliquotaIcms",
                table: "ImpostosNfes");

            migrationBuilder.AddColumn<string>(
                name: "CFOP",
                table: "Produtos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Unidade",
                table: "Produtos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
