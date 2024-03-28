using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class createdparenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IssueUrl",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Issues",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ParentIssues",
                columns: table => new
                {
                    ParentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentIssues", x => x.ParentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ParentId",
                table: "Issues",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_ParentIssues_ParentId",
                table: "Issues");

            migrationBuilder.DropTable(
                name: "ParentIssues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_ParentId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Issues");

            migrationBuilder.AlterColumn<string>(
                name: "IssueUrl",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
