using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProfessorAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "perfil",
                table: "professores",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Professor");

                migrationBuilder.Sql("UPDATE professores SET perfil = 'Admin' WHERE user_name = 'admin01';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "perfil",
                table: "professores");
        }
    }
}
