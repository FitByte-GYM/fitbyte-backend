using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym_FitByte.Migrations
{
    /// <inheritdoc />
    public partial class Actual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoCliente",
                table: "Membresias",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "MontoPagado",
                table: "Membresias",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoCliente",
                table: "Membresias");

            migrationBuilder.DropColumn(
                name: "MontoPagado",
                table: "Membresias");
        }
    }
}
