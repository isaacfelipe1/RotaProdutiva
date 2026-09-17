using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RotaProdutiva.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddContatoInscricao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Usuarios_JovemId",
                table: "Inscricoes");

            migrationBuilder.DropIndex(
                name: "IX_Inscricoes_JovemId_CursoId",
                table: "Inscricoes");

            migrationBuilder.DropColumn(
                name: "JovemId",
                table: "Inscricoes");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Inscricoes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Inscricoes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WhatsApp",
                table: "Inscricoes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_Email_CursoId",
                table: "Inscricoes",
                columns: new[] { "Email", "CursoId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Inscricoes_Email_CursoId",
                table: "Inscricoes");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Inscricoes");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Inscricoes");

            migrationBuilder.DropColumn(
                name: "WhatsApp",
                table: "Inscricoes");

            migrationBuilder.AddColumn<Guid>(
                name: "JovemId",
                table: "Inscricoes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_JovemId_CursoId",
                table: "Inscricoes",
                columns: new[] { "JovemId", "CursoId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Usuarios_JovemId",
                table: "Inscricoes",
                column: "JovemId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
