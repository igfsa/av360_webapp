using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CriterioProfessor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "nativo",
                table: "criterios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "professor_id",
                table: "criterios",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "criterios",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "nativo", "professor_id" },
                values: new object[] { true, null });

            migrationBuilder.UpdateData(
                table: "criterios",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "nativo", "professor_id" },
                values: new object[] { true, null });

            migrationBuilder.UpdateData(
                table: "criterios",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "nativo", "professor_id" },
                values: new object[] { true, null });

            migrationBuilder.UpdateData(
                table: "criterios",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "nativo", "professor_id" },
                values: new object[] { true, null });

            migrationBuilder.UpdateData(
                table: "criterios",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "nativo", "professor_id" },
                values: new object[] { true, null });

            migrationBuilder.UpdateData(
                table: "criterios",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "nativo", "professor_id" },
                values: new object[] { true, null });

            migrationBuilder.UpdateData(
                table: "criterios",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "nativo", "professor_id" },
                values: new object[] { true, null });

            migrationBuilder.UpdateData(
                table: "criterios",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "nativo", "professor_id" },
                values: new object[] { true, null });

            migrationBuilder.UpdateData(
                table: "criterios",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "nativo", "professor_id" },
                values: new object[] { true, null });

            migrationBuilder.CreateIndex(
                name: "ix_criterios_professor_id",
                table: "criterios",
                column: "professor_id");

            migrationBuilder.AddForeignKey(
                name: "fk_criterios_professores_professor_id",
                table: "criterios",
                column: "professor_id",
                principalTable: "professores",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(
                "UPDATE criterios " + 
                "SET professor_id = 1 "+
                "WHERE criterios.id > 9;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_criterios_professores_professor_id",
                table: "criterios");

            migrationBuilder.DropIndex(
                name: "ix_criterios_professor_id",
                table: "criterios");

            migrationBuilder.DropColumn(
                name: "nativo",
                table: "criterios");

            migrationBuilder.DropColumn(
                name: "professor_id",
                table: "criterios");
        }
    }
}
