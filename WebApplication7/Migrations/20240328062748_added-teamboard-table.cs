using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class addedteamboardtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamBoardId",
                table: "Issues",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TeamBoards",
                columns: table => new
                {
                    TeamBoardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamBoards", x => x.TeamBoardId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TeamBoardId",
                table: "Issues",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_TeamBoards_TeamBoardId",
                table: "Issues");

            migrationBuilder.DropTable(
                name: "TeamBoards");

            migrationBuilder.DropIndex(
                name: "IX_Issues_TeamBoardId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "TeamBoardId",
                table: "Issues");
        }
    }
}
