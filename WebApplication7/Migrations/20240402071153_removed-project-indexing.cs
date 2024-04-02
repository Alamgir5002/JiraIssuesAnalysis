using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class removedprojectindexing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_Key",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Key",
                table: "Projects",
                column: "Key",
                unique: true);
        }
    }
}
