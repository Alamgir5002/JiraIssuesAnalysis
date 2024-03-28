using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class removedissuetypeidcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_IssueTypes_IssueTypeId",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueTypes",
                table: "IssueTypes");

            migrationBuilder.DropIndex(
                name: "IX_IssueTypes_Id",
                table: "IssueTypes");

            migrationBuilder.DropColumn(
                name: "IssueTypeId",
                table: "IssueTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "IssueTypes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IssueTypeId",
                table: "Issues",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueTypes",
                table: "IssueTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_IssueTypes_Id",
                table: "IssueTypes",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_IssueTypes_IssueTypeId",
                table: "Issues",
                column: "IssueTypeId",
                principalTable: "IssueTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_IssueTypes_IssueTypeId",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueTypes",
                table: "IssueTypes");

            migrationBuilder.DropIndex(
                name: "IX_IssueTypes_Id",
                table: "IssueTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "IssueTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "IssueTypeId",
                table: "IssueTypes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "IssueTypeId",
                table: "Issues",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueTypes",
                table: "IssueTypes",
                column: "IssueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueTypes_Id",
                table: "IssueTypes",
                column: "Id",
                unique: true,
                filter: "[Id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_IssueTypes_IssueTypeId",
                table: "Issues",
                column: "IssueTypeId",
                principalTable: "IssueTypes",
                principalColumn: "IssueTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
