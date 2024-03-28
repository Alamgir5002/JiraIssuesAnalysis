using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class removedextracolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_ParentIssues_ParentId",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentIssues",
                table: "ParentIssues");

            migrationBuilder.DropIndex(
                name: "IX_ParentIssues_ParentId",
                table: "ParentIssues");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ParentIssues");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ParentIssues",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                table: "Issues",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentIssues",
                table: "ParentIssues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_ParentIssues_ParentId",
                table: "Issues",
                column: "ParentId",
                principalTable: "ParentIssues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_ParentIssues_ParentId",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentIssues",
                table: "ParentIssues");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ParentIssues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "ParentIssues",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Issues",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentIssues",
                table: "ParentIssues",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentIssues_ParentId",
                table: "ParentIssues",
                column: "ParentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_ParentIssues_ParentId",
                table: "Issues",
                column: "ParentId",
                principalTable: "ParentIssues",
                principalColumn: "ParentId");
        }
    }
}
