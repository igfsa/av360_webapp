using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Sessao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Secoes");

            migrationBuilder.RenameColumn(
                name: "SecaoId",
                table: "NotasFinais",
                newName: "SessaoId");

            migrationBuilder.RenameIndex(
                name: "IX_NotasFinais_SecaoId_DeviceHash",
                table: "NotasFinais",
                newName: "IX_NotasFinais_SessaoId_DeviceHash");

            migrationBuilder.RenameIndex(
                name: "IX_NotasFinais_SecaoId_AvaliadorId",
                table: "NotasFinais",
                newName: "IX_NotasFinais_SessaoId_AvaliadorId");

            migrationBuilder.CreateTable(
                name: "Sessoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TurmaId = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TokenPublico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessoes");

            migrationBuilder.RenameColumn(
                name: "SessaoId",
                table: "NotasFinais",
                newName: "SecaoId");

            migrationBuilder.RenameIndex(
                name: "IX_NotasFinais_SessaoId_DeviceHash",
                table: "NotasFinais",
                newName: "IX_NotasFinais_SecaoId_DeviceHash");

            migrationBuilder.RenameIndex(
                name: "IX_NotasFinais_SessaoId_AvaliadorId",
                table: "NotasFinais",
                newName: "IX_NotasFinais_SecaoId_AvaliadorId");

            migrationBuilder.CreateTable(
                name: "Secoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TokenPublico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TurmaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
