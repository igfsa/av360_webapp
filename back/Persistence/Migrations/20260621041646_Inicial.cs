using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aluno_grupo",
                columns: table => new
                {
                    aluno_id = table.Column<int>(type: "integer", nullable: false),
                    grupo_id = table.Column<int>(type: "integer", nullable: false),
                    turma_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aluno_grupo", x => new { x.aluno_id, x.grupo_id });
                });

            migrationBuilder.CreateTable(
                name: "alunos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alunos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "criterios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_criterios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "professores",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    senha_hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_professores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "turmas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cod = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nota_max = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_turmas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(type: "text", nullable: false),
                    expira_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revogado = table.Column<bool>(type: "boolean", nullable: false),
                    professor_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_professores_professor_id",
                        column: x => x.professor_id,
                        principalTable: "professores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aluno_turma",
                columns: table => new
                {
                    alunos_id = table.Column<int>(type: "integer", nullable: false),
                    turmas_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aluno_turma", x => new { x.alunos_id, x.turmas_id });
                    table.ForeignKey(
                        name: "fk_aluno_turma_alunos_alunos_id",
                        column: x => x.alunos_id,
                        principalTable: "alunos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_aluno_turma_turmas_turmas_id",
                        column: x => x.turmas_id,
                        principalTable: "turmas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "criterio_turma",
                columns: table => new
                {
                    criterios_id = table.Column<int>(type: "integer", nullable: false),
                    turmas_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_criterio_turma", x => new { x.criterios_id, x.turmas_id });
                    table.ForeignKey(
                        name: "fk_criterio_turma_criterios_criterios_id",
                        column: x => x.criterios_id,
                        principalTable: "criterios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_criterio_turma_turmas_turmas_id",
                        column: x => x.turmas_id,
                        principalTable: "turmas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grupos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    turma_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grupos", x => x.id);
                    table.ForeignKey(
                        name: "fk_grupos_turmas_turma_id",
                        column: x => x.turma_id,
                        principalTable: "turmas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sessoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    turma_id = table.Column<int>(type: "integer", nullable: false),
                    data_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_fim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    token_publico = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sessoes", x => x.id);
                    table.ForeignKey(
                        name: "fk_sessoes_turmas_turma_id",
                        column: x => x.turma_id,
                        principalTable: "turmas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notas_finais",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sessao_id = table.Column<int>(type: "integer", nullable: false),
                    avaliador_id = table.Column<int>(type: "integer", nullable: false),
                    grupo_id = table.Column<int>(type: "integer", nullable: false),
                    device_hash = table.Column<string>(type: "character varying(65)", maxLength: 65, nullable: false),
                    data_envio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notas_finais", x => x.id);
                    table.ForeignKey(
                        name: "fk_notas_finais_alunos_avaliador_id",
                        column: x => x.avaliador_id,
                        principalTable: "alunos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notas_finais_grupos_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notas_finais_sessoes_sessao_id",
                        column: x => x.sessao_id,
                        principalTable: "sessoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "notas_parciais",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nota_final_id = table.Column<int>(type: "integer", nullable: false),
                    avaliado_id = table.Column<int>(type: "integer", nullable: false),
                    criterio_id = table.Column<int>(type: "integer", nullable: false),
                    nota = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notas_parciais", x => x.id);
                    table.ForeignKey(
                        name: "fk_notas_parciais_alunos_avaliado_id",
                        column: x => x.avaliado_id,
                        principalTable: "alunos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notas_parciais_criterios_criterio_id",
                        column: x => x.criterio_id,
                        principalTable: "criterios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notas_parciais_notas_finais_nota_final_id",
                        column: x => x.nota_final_id,
                        principalTable: "notas_finais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.InsertData(
                table: "criterios",
                columns: new[] { "id", "nome" },
                values: new object[,]
                {
                    { 1, "Nível de Participação" },
                    { 2, "Pontualidade na Entrega de Tarefas" },
                    { 3, "Capacidade de Trabalhar em Equipe" },
                    { 4, "Controle Emocional" },
                    { 5, "Disposição para Compartilhar Tarefas" },
                    { 6, "Respeito às Individualidades" },
                    { 7, "Responsabilidade" },
                    { 8, "Criatividade" },
                    { 9, "Conhecimento Teórico" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_aluno_turma_turmas_id",
                table: "aluno_turma",
                column: "turmas_id");

            migrationBuilder.CreateIndex(
                name: "ix_alunos_resultados_resultado_grupo_id",
                table: "alunos_resultados",
                column: "resultado_grupo_id");

            migrationBuilder.CreateIndex(
                name: "ix_alunos_resultados_resultado_sessao_id",
                table: "alunos_resultados",
                column: "resultado_sessao_id");

            migrationBuilder.CreateIndex(
                name: "ix_criterio_turma_turmas_id",
                table: "criterio_turma",
                column: "turmas_id");

            migrationBuilder.CreateIndex(
                name: "ix_criterios_resultados_resultado_sessao_id",
                table: "criterios_resultados",
                column: "resultado_sessao_id");

            migrationBuilder.CreateIndex(
                name: "ix_grupos_turma_id",
                table: "grupos",
                column: "turma_id");

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
                name: "ix_notas_finais_avaliador_id",
                table: "notas_finais",
                column: "avaliador_id");

            migrationBuilder.CreateIndex(
                name: "ix_notas_finais_grupo_id",
                table: "notas_finais",
                column: "grupo_id");

            migrationBuilder.CreateIndex(
                name: "ix_notas_finais_sessao_id",
                table: "notas_finais",
                column: "sessao_id");

            migrationBuilder.CreateIndex(
                name: "ix_notas_finais_sessao_id_avaliador_id",
                table: "notas_finais",
                columns: new[] { "sessao_id", "avaliador_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_notas_finais_sessao_id_device_hash",
                table: "notas_finais",
                columns: new[] { "sessao_id", "device_hash" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_notas_parciais_avaliado_id",
                table: "notas_parciais",
                column: "avaliado_id")
                .Annotation("Npgsql:IndexInclude", new[] { "nota", "criterio_id" });

            migrationBuilder.CreateIndex(
                name: "ix_notas_parciais_avaliado_id_criterio_id",
                table: "notas_parciais",
                columns: new[] { "avaliado_id", "criterio_id" });

            migrationBuilder.CreateIndex(
                name: "ix_notas_parciais_criterio_id",
                table: "notas_parciais",
                column: "criterio_id");

            migrationBuilder.CreateIndex(
                name: "ix_notas_parciais_nota_final_id_avaliado_id_criterio_id",
                table: "notas_parciais",
                columns: new[] { "nota_final_id", "avaliado_id", "criterio_id" },
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
                name: "ix_professores_user_name",
                table: "professores",
                column: "user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_professor_id",
                table: "refresh_tokens",
                column: "professor_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sessoes_turma_id",
                table: "sessoes",
                column: "turma_id");

            migrationBuilder.CreateIndex(
                name: "ix_sessoes_resultados_sessao_id",
                table: "sessoes_resultados",
                column: "sessao_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aluno_grupo");

            migrationBuilder.DropTable(
                name: "aluno_turma");

            migrationBuilder.DropTable(
                name: "alunos_resultados");

            migrationBuilder.DropTable(
                name: "criterio_turma");

            migrationBuilder.DropTable(
                name: "criterios_resultados");

            migrationBuilder.DropTable(
                name: "notas_parciais");

            migrationBuilder.DropTable(
                name: "notas_parciais_resultados");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "grupos_resultados");

            migrationBuilder.DropTable(
                name: "criterios");

            migrationBuilder.DropTable(
                name: "notas_finais");

            migrationBuilder.DropTable(
                name: "nota_finais_resultados");

            migrationBuilder.DropTable(
                name: "professores");

            migrationBuilder.DropTable(
                name: "alunos");

            migrationBuilder.DropTable(
                name: "grupos");

            migrationBuilder.DropTable(
                name: "sessoes_resultados");

            migrationBuilder.DropTable(
                name: "sessoes");

            migrationBuilder.DropTable(
                name: "turmas");
        }
    }
}
