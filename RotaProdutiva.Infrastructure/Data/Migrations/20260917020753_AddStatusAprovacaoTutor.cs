using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RotaProdutiva.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusAprovacaoTutor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusAprovacao",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusAprovacao",
                table: "Usuarios");
        }
    }
}
