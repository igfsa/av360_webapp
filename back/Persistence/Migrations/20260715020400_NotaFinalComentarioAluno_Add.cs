using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NotaFinalComentarioAluno_Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "comentario_aluno",
                table: "notas_finais",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "comentario_aluno",
                table: "nota_finais_resultados",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comentario_aluno",
                table: "notas_finais");

            migrationBuilder.DropColumn(
                name: "comentario_aluno",
                table: "nota_finais_resultados");
        }
    }
}
