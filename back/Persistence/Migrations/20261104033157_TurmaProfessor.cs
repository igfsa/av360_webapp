using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TurmaProfessor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "professor_id",
                table: "turmas",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "ix_turmas_professor_id",
                table: "turmas",
                column: "professor_id");

            migrationBuilder.AddForeignKey(
                name: "fk_turmas_professores_professor_id",
                table: "turmas",
                column: "professor_id",
                principalTable: "professores",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_turmas_professores_professor_id",
                table: "turmas");

            migrationBuilder.DropIndex(
                name: "ix_turmas_professor_id",
                table: "turmas");

            migrationBuilder.DropColumn(
                name: "professor_id",
                table: "turmas");
        }
    }
}
