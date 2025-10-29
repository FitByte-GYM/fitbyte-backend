using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym_FitByte.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaMembresias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Membresias_Clientes_ClienteId",
                table: "Membresias");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "PreRegistros");

            migrationBuilder.DropIndex(
                name: "IX_Membresias_ClienteId",
                table: "Membresias");

            migrationBuilder.RenameColumn(
                name: "FechaInicio",
                table: "Membresias",
                newName: "FechaVencimiento");

            migrationBuilder.RenameColumn(
                name: "FechaFin",
                table: "Membresias",
                newName: "FechaRegistro");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Membresias",
                newName: "Edad");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Membresias",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Membresias",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EnfermedadesOLesiones",
                table: "Membresias",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FormaPago",
                table: "Membresias",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Membresias",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Rutina",
                table: "Membresias",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Membresias",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Membresias");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Membresias");

            migrationBuilder.DropColumn(
                name: "EnfermedadesOLesiones",
                table: "Membresias");

            migrationBuilder.DropColumn(
                name: "FormaPago",
                table: "Membresias");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Membresias");

            migrationBuilder.DropColumn(
                name: "Rutina",
                table: "Membresias");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Membresias");

            migrationBuilder.RenameColumn(
                name: "FechaVencimiento",
                table: "Membresias",
                newName: "FechaInicio");

            migrationBuilder.RenameColumn(
                name: "FechaRegistro",
                table: "Membresias",
                newName: "FechaFin");

            migrationBuilder.RenameColumn(
                name: "Edad",
                table: "Membresias",
                newName: "ClienteId");

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Apellido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodigoCliente = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Correo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FotoUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MembresiaActiva = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PreRegistros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Apellido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Correo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Objetivo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValidadoPorAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRegistros", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Membresias_ClienteId",
                table: "Membresias",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Membresias_Clientes_ClienteId",
                table: "Membresias",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
