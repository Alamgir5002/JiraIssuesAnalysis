using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class removedTeamBoardKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_TeamBoards_TeamBoardId",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamBoards",
                table: "TeamBoards");

            migrationBuilder.DropIndex(
                name: "IX_TeamBoards_Id",
                table: "TeamBoards");

            migrationBuilder.DropColumn(
                name: "TeamBoardId",
                table: "TeamBoards");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TeamBoards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TeamBoardId",
                table: "Issues",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamBoards",
                table: "TeamBoards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_TeamBoards_TeamBoardId",
                table: "Issues",
                column: "TeamBoardId",
                principalTable: "TeamBoards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_TeamBoards_TeamBoardId",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamBoards",
                table: "TeamBoards");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TeamBoards",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "TeamBoardId",
                table: "TeamBoards",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "TeamBoardId",
                table: "Issues",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamBoards",
                table: "TeamBoards",
                column: "TeamBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamBoards_Id",
                table: "TeamBoards",
                column: "Id",
                unique: true,
                filter: "[Id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_TeamBoards_TeamBoardId",
                table: "Issues",
                column: "TeamBoardId",
                principalTable: "TeamBoards",
                principalColumn: "TeamBoardId");
        }
    }
}
