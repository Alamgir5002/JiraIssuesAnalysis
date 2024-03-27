using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class createdrelationwithestimates1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimatedAndSpentTimes_Issue_IssueId",
                table: "EstimatedAndSpentTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_IssueType_IssueTypeId",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Parent_ParentId",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_TeamBoard_TeamBoardId",
                table: "Issue");

            migrationBuilder.DropTable(
                name: "IssueType");

            migrationBuilder.DropTable(
                name: "Parent");

            migrationBuilder.DropTable(
                name: "Release");

            migrationBuilder.DropTable(
                name: "TeamBoard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Issue",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_IssueTypeId",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_ParentId",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_TeamBoardId",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "IssueTypeId",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "TeamBoardId",
                table: "Issue");

            migrationBuilder.RenameTable(
                name: "Issue",
                newName: "Issues");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Issues",
                table: "Issues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimatedAndSpentTimes_Issues_IssueId",
                table: "EstimatedAndSpentTimes",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimatedAndSpentTimes_Issues_IssueId",
                table: "EstimatedAndSpentTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Issues",
                table: "Issues");

            migrationBuilder.RenameTable(
                name: "Issues",
                newName: "Issue");

            migrationBuilder.AddColumn<string>(
                name: "IssueTypeId",
                table: "Issue",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "Issue",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamBoardId",
                table: "Issue",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Issue",
                table: "Issue",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "IssueType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTask = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parent",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Release",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssueId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Released = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Release", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Release_Issue_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issue",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamBoard",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamBoard", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issue_IssueTypeId",
                table: "Issue",
                column: "IssueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_ParentId",
                table: "Issue",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_TeamBoardId",
                table: "Issue",
                column: "TeamBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Release_IssueId",
                table: "Release",
                column: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimatedAndSpentTimes_Issue_IssueId",
                table: "EstimatedAndSpentTimes",
                column: "IssueId",
                principalTable: "Issue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_IssueType_IssueTypeId",
                table: "Issue",
                column: "IssueTypeId",
                principalTable: "IssueType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Parent_ParentId",
                table: "Issue",
                column: "ParentId",
                principalTable: "Parent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_TeamBoard_TeamBoardId",
                table: "Issue",
                column: "TeamBoardId",
                principalTable: "TeamBoard",
                principalColumn: "Id");
        }
    }
}
