using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class device_hashajuste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "device_hash",
                table: "notas_finais",
                type: "character varying(65)",
                maxLength: 65,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(65)",
                oldFixedLength: true,
                oldMaxLength: 65);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "device_hash",
                table: "notas_finais",
                type: "character(65)",
                fixedLength: true,
                maxLength: 65,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(65)",
                oldMaxLength: 65);
        }
    }
}
