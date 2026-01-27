using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Grupo_Secao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotasFinais_Alunos_AlunoId",
                table: "NotasFinais");

            migrationBuilder.DropForeignKey(
                name: "FK_NotasFinais_Criterios_CriterioId",
                table: "NotasFinais");

            migrationBuilder.DropForeignKey(
                name: "FK_NotasFinais_Turmas_TurmaId",
                table: "NotasFinais");

            migrationBuilder.DropForeignKey(
                name: "FK_NotasParciais_Alunos_AlunoId",
                table: "NotasParciais");

            migrationBuilder.DropForeignKey(
                name: "FK_NotasParciais_Alunos_AvaliadorId",
                table: "NotasParciais");

            migrationBuilder.DropForeignKey(
                name: "FK_NotasParciais_Criterios_CriterioId",
                table: "NotasParciais");

            migrationBuilder.DropForeignKey(
                name: "FK_NotasParciais_NotasFinais_NotaFinalId",
                table: "NotasParciais");

            migrationBuilder.DropForeignKey(
                name: "FK_NotasParciais_Turmas_TurmaId",
                table: "NotasParciais");

            migrationBuilder.DropIndex(
                name: "IX_NotasParciais_AvaliadorId",
                table: "NotasParciais");

            migrationBuilder.DropIndex(
                name: "IX_NotasParciais_CriterioId",
                table: "NotasParciais");

            migrationBuilder.DropIndex(
                name: "IX_NotasParciais_NotaFinalId",
                table: "NotasParciais");

            migrationBuilder.DropIndex(
                name: "IX_NotasParciais_TurmaId",
                table: "NotasParciais");

            migrationBuilder.DropIndex(
                name: "IX_NotasFinais_CriterioId",
                table: "NotasFinais");

            migrationBuilder.DropIndex(
                name: "IX_NotasFinais_TurmaId",
                table: "NotasFinais");

            migrationBuilder.DropColumn(
                name: "AvaliadorId",
                table: "NotasParciais");

            migrationBuilder.DropColumn(
                name: "Nota",
                table: "NotasFinais");

            migrationBuilder.RenameColumn(
                name: "TurmaId",
                table: "NotasParciais",
                newName: "AvaliadoId");

            migrationBuilder.RenameColumn(
                name: "TurmaId",
                table: "NotasFinais",
                newName: "SecaoId");

            migrationBuilder.RenameColumn(
                name: "CriterioId",
                table: "NotasFinais",
                newName: "GrupoId");

            migrationBuilder.AlterColumn<int>(
                name: "AlunoId",
                table: "NotasParciais",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AlunoId",
                table: "NotasFinais",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AvaliadorId",
                table: "NotasFinais",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEnvio",
                table: "NotasFinais",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeviceHash",
                table: "NotasFinais",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlunoGrupo",
                columns: table => new
                {
                    AlunoId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlunoGrupo", x => new { x.AlunoId, x.GrupoId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TurmaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Secoes",
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
                    table.PrimaryKey("PK_Secoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_NotasFinais_SecaoId_AvaliadorId",
                table: "NotasFinais",
                columns: new[] { "SecaoId", "AvaliadorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotasFinais_SecaoId_DeviceHash",
                table: "NotasFinais",
                columns: new[] { "SecaoId", "DeviceHash" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NotasFinais_Alunos_AlunoId",
                table: "NotasFinais",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotasParciais_Alunos_AlunoId",
                table: "NotasParciais",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotasFinais_Alunos_AlunoId",
                table: "NotasFinais");

            migrationBuilder.DropForeignKey(
                name: "FK_NotasParciais_Alunos_AlunoId",
                table: "NotasParciais");

            migrationBuilder.DropTable(
                name: "AlunoGrupo");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "Secoes");

            migrationBuilder.DropIndex(
                name: "IX_NotasFinais_SecaoId_AvaliadorId",
                table: "NotasFinais");

            migrationBuilder.DropIndex(
                name: "IX_NotasFinais_SecaoId_DeviceHash",
                table: "NotasFinais");

            migrationBuilder.DropColumn(
                name: "AvaliadorId",
                table: "NotasFinais");

            migrationBuilder.DropColumn(
                name: "DataEnvio",
                table: "NotasFinais");

            migrationBuilder.DropColumn(
                name: "DeviceHash",
                table: "NotasFinais");

            migrationBuilder.RenameColumn(
                name: "AvaliadoId",
                table: "NotasParciais",
                newName: "TurmaId");

            migrationBuilder.RenameColumn(
                name: "SecaoId",
                table: "NotasFinais",
                newName: "TurmaId");

            migrationBuilder.RenameColumn(
                name: "GrupoId",
                table: "NotasFinais",
                newName: "CriterioId");

            migrationBuilder.AlterColumn<int>(
                name: "AlunoId",
                table: "NotasParciais",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvaliadorId",
                table: "NotasParciais",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AlunoId",
                table: "NotasFinais",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Nota",
                table: "NotasFinais",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_NotasParciais_AvaliadorId",
                table: "NotasParciais",
                column: "AvaliadorId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasParciais_CriterioId",
                table: "NotasParciais",
                column: "CriterioId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasParciais_NotaFinalId",
                table: "NotasParciais",
                column: "NotaFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasParciais_TurmaId",
                table: "NotasParciais",
                column: "TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasFinais_CriterioId",
                table: "NotasFinais",
                column: "CriterioId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasFinais_TurmaId",
                table: "NotasFinais",
                column: "TurmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotasFinais_Alunos_AlunoId",
                table: "NotasFinais",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotasFinais_Criterios_CriterioId",
                table: "NotasFinais",
                column: "CriterioId",
                principalTable: "Criterios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotasFinais_Turmas_TurmaId",
                table: "NotasFinais",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotasParciais_Alunos_AlunoId",
                table: "NotasParciais",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotasParciais_Alunos_AvaliadorId",
                table: "NotasParciais",
                column: "AvaliadorId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotasParciais_Criterios_CriterioId",
                table: "NotasParciais",
                column: "CriterioId",
                principalTable: "Criterios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotasParciais_NotasFinais_NotaFinalId",
                table: "NotasParciais",
                column: "NotaFinalId",
                principalTable: "NotasFinais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotasParciais_Turmas_TurmaId",
                table: "NotasParciais",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
