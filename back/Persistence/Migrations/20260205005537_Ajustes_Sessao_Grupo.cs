using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Ajustes_Sessao_Grupo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Turmas_TurmaId",
                table: "Alunos");

            migrationBuilder.DropForeignKey(
                name: "FK_Criterios_Turmas_TurmaId",
                table: "Criterios");

            migrationBuilder.DropIndex(
                name: "IX_Criterios_TurmaId",
                table: "Criterios");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_TurmaId",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "TurmaId",
                table: "Criterios");

            migrationBuilder.DropColumn(
                name: "TurmaId",
                table: "Alunos");

            migrationBuilder.AlterColumn<string>(
                name: "TokenPublico",
                table: "Sessoes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "Sessoes",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.CreateIndex(
                name: "IX_NotasParciais_NotaFinalId_AvaliadoId_CriterioId",
                table: "NotasParciais",
                columns: new[] { "NotaFinalId", "AvaliadoId", "CriterioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_TurmaId",
                table: "Grupos",
                column: "TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_CriterioTurma_TurmaId",
                table: "CriterioTurma",
                column: "TurmaId");

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
                name: "FK_CriterioTurma_Turmas_TurmaId",
                table: "CriterioTurma",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grupos_Turmas_TurmaId",
                table: "Grupos",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotasParciais_NotasFinais_NotaFinalId",
                table: "NotasParciais",
                column: "NotaFinalId",
                principalTable: "NotasFinais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlunoTurma_Turmas_TurmaId",
                table: "AlunoTurma");

            migrationBuilder.DropForeignKey(
                name: "FK_CriterioTurma_Turmas_TurmaId",
                table: "CriterioTurma");

            migrationBuilder.DropForeignKey(
                name: "FK_Grupos_Turmas_TurmaId",
                table: "Grupos");

            migrationBuilder.DropForeignKey(
                name: "FK_NotasParciais_NotasFinais_NotaFinalId",
                table: "NotasParciais");

            migrationBuilder.DropIndex(
                name: "IX_NotasParciais_NotaFinalId_AvaliadoId_CriterioId",
                table: "NotasParciais");

            migrationBuilder.DropIndex(
                name: "IX_Grupos_TurmaId",
                table: "Grupos");

            migrationBuilder.DropIndex(
                name: "IX_CriterioTurma_TurmaId",
                table: "CriterioTurma");

            migrationBuilder.DropIndex(
                name: "IX_AlunoTurma_TurmaId",
                table: "AlunoTurma");

            migrationBuilder.AlterColumn<Guid>(
                name: "TokenPublico",
                table: "Sessoes",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "Sessoes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TurmaId",
                table: "Criterios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TurmaId",
                table: "Alunos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Criterios_TurmaId",
                table: "Criterios",
                column: "TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_TurmaId",
                table: "Alunos",
                column: "TurmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Turmas_TurmaId",
                table: "Alunos",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Criterios_Turmas_TurmaId",
                table: "Criterios",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id");
        }
    }
}
