using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ObjetosResultados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sessoes_resultados",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sessao_id = table.Column<int>(type: "integer", nullable: false),
                    turma_id = table.Column<int>(type: "integer", nullable: false),
                    turma_cod = table.Column<string>(type: "text", nullable: false),
                    nota_maxima = table.Column<decimal>(type: "numeric", nullable: false),
                    data_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_fim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sessoes_resultados", x => x.id);
                    table.ForeignKey(
                        name: "fk_sessoes_resultados_sessoes_sessao_id",
                        column: x => x.sessao_id,
                        principalTable: "sessoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "criterios_resultados",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    criterio_id = table.Column<int>(type: "integer", nullable: false),
                    resultado_sessao_id = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_criterios_resultados", x => x.id);
                    table.ForeignKey(
                        name: "fk_criterios_resultados_sessoes_resultados_resultado_sessao_id",
                        column: x => x.resultado_sessao_id,
                        principalTable: "sessoes_resultados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "grupos_resultados",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grupo_id = table.Column<int>(type: "integer", nullable: false),
                    resultado_sessao_id = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grupos_resultados", x => x.id);
                    table.ForeignKey(
                        name: "fk_grupos_resultados_sessoes_resultados_resultado_sessao_id",
                        column: x => x.resultado_sessao_id,
                        principalTable: "sessoes_resultados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nota_finais_resultados",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nota_final_id = table.Column<int>(type: "integer", nullable: false),
                    resultado_sessao_id = table.Column<int>(type: "integer", nullable: false),
                    avaliador_res_id = table.Column<int>(type: "integer", nullable: false),
                    avaliador_nome = table.Column<string>(type: "text", nullable: false),
                    grupo_res_id = table.Column<int>(type: "integer", nullable: false),
                    grupo_nome = table.Column<string>(type: "text", nullable: false),
                    data_envio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nota_finais_resultados", x => x.id);
                    table.ForeignKey(
                        name: "fk_nota_finais_resultados_sessoes_resultados_resultado_sessao_",
                        column: x => x.resultado_sessao_id,
                        principalTable: "sessoes_resultados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alunos_resultados",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    aluno_id = table.Column<int>(type: "integer", nullable: false),
                    resultado_sessao_id = table.Column<int>(type: "integer", nullable: false),
                    resultado_grupo_id = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alunos_resultados", x => x.id);
                    table.ForeignKey(
                        name: "fk_alunos_resultados_grupos_resultados_resultado_grupo_id",
                        column: x => x.resultado_grupo_id,
                        principalTable: "grupos_resultados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_alunos_resultados_sessoes_resultados_resultado_sessao_id",
                        column: x => x.resultado_sessao_id,
                        principalTable: "sessoes_resultados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notas_parciais_resultados",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nota_parcial_id = table.Column<int>(type: "integer", nullable: false),
                    resultado_nota_final_id = table.Column<int>(type: "integer", nullable: false),
                    avaliado_res_id = table.Column<int>(type: "integer", nullable: false),
                    avaliado_res_nome = table.Column<string>(type: "text", nullable: false),
                    criterio_res_id = table.Column<int>(type: "integer", nullable: false),
                    criterio_res_nome = table.Column<string>(type: "text", nullable: false),
                    nota = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notas_parciais_resultados", x => x.id);
                    table.ForeignKey(
                        name: "fk_notas_parciais_resultados_nota_finais_resultados_resultado_",
                        column: x => x.resultado_nota_final_id,
                        principalTable: "nota_finais_resultados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_alunos_resultados_resultado_grupo_id",
                table: "alunos_resultados",
                column: "resultado_grupo_id");

            migrationBuilder.CreateIndex(
                name: "ix_alunos_resultados_resultado_sessao_id",
                table: "alunos_resultados",
                column: "resultado_sessao_id");

            migrationBuilder.CreateIndex(
                name: "ix_criterios_resultados_resultado_sessao_id",
                table: "criterios_resultados",
                column: "resultado_sessao_id");

            migrationBuilder.CreateIndex(
                name: "ix_grupos_resultados_resultado_sessao_id",
                table: "grupos_resultados",
                column: "resultado_sessao_id");

            migrationBuilder.CreateIndex(
                name: "ix_nota_finais_resultados_resultado_sessao_id",
                table: "nota_finais_resultados",
                column: "resultado_sessao_id");

            migrationBuilder.CreateIndex(
                name: "ix_nota_finais_resultados_resultado_sessao_id_avaliador_res_id",
                table: "nota_finais_resultados",
                columns: new[] { "resultado_sessao_id", "avaliador_res_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_notas_parciais_resultados_avaliado_res_id",
                table: "notas_parciais_resultados",
                column: "avaliado_res_id")
                .Annotation("Npgsql:IndexInclude", new[] { "nota", "criterio_res_id" });

            migrationBuilder.CreateIndex(
                name: "ix_notas_parciais_resultados_avaliado_res_id_criterio_res_id",
                table: "notas_parciais_resultados",
                columns: new[] { "avaliado_res_id", "criterio_res_id" });

            migrationBuilder.CreateIndex(
                name: "ix_notas_parciais_resultados_criterio_res_id",
                table: "notas_parciais_resultados",
                column: "criterio_res_id");

            migrationBuilder.CreateIndex(
                name: "ix_notas_parciais_resultados_resultado_nota_final_id_avaliado_",
                table: "notas_parciais_resultados",
                columns: new[] { "resultado_nota_final_id", "avaliado_res_id", "criterio_res_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sessoes_resultados_sessao_id",
                table: "sessoes_resultados",
                column: "sessao_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alunos_resultados");

            migrationBuilder.DropTable(
                name: "criterios_resultados");

            migrationBuilder.DropTable(
                name: "notas_parciais_resultados");

            migrationBuilder.DropTable(
                name: "grupos_resultados");

            migrationBuilder.DropTable(
                name: "nota_finais_resultados");

            migrationBuilder.DropTable(
                name: "sessoes_resultados");
        }
    }
}
