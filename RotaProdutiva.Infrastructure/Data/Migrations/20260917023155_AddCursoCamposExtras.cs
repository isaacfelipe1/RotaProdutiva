using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RotaProdutiva.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCursoCamposExtras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CargaHoraria",
                table: "Cursos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "Cursos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Local",
                table: "Cursos",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modalidade",
                table: "Cursos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Vagas",
                table: "Cursos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CargaHoraria",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "Local",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "Modalidade",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "Vagas",
                table: "Cursos");
        }
    }
}
