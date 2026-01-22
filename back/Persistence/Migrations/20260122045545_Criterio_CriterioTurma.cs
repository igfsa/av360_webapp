using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Criterio_CriterioTurma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlunoTurma_Turmas_TurmaId",
                table: "AlunoTurma");

            migrationBuilder.DropForeignKey(
                name: "FK_Criterios_Turmas_TurmaId",
                table: "Criterios");

            migrationBuilder.DropIndex(
                name: "IX_AlunoTurma_TurmaId",
                table: "AlunoTurma");

            migrationBuilder.AlterColumn<int>(
                name: "TurmaId",
                table: "Criterios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "CriterioTurma",
                columns: table => new
                {
                    CriterioId = table.Column<int>(type: "int", nullable: false),
                    TurmaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriterioTurma", x => new { x.CriterioId, x.TurmaId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Criterios_Turmas_TurmaId",
                table: "Criterios",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criterios_Turmas_TurmaId",
                table: "Criterios");

            migrationBuilder.DropTable(
                name: "CriterioTurma");

            migrationBuilder.AlterColumn<int>(
                name: "TurmaId",
                table: "Criterios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlunoTurma_TurmaId",
                table: "AlunoTurma",
                column: "TurmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlunoTurma_Turmas_TurmaId",
                table: "AlunoTurma",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Criterios_Turmas_TurmaId",
                table: "Criterios",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
