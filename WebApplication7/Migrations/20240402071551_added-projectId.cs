using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class addedprojectId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Key",
                table: "Projects",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Key",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");
        }
    }
}
