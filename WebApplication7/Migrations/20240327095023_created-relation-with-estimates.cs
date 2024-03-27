using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class createdrelationwithestimates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parent", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Issue",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssueTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResolvedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoryPoints = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeamBoardId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IssueUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductivityRatio = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issue_IssueType_IssueTypeId",
                        column: x => x.IssueTypeId,
                        principalTable: "IssueType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Issue_Parent_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Issue_TeamBoard_TeamBoardId",
                        column: x => x.TeamBoardId,
                        principalTable: "TeamBoard",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EstimatedAndSpentTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AggregatedTimeSpent = table.Column<double>(type: "float", nullable: false),
                    AggregateTimeEstimate = table.Column<double>(type: "float", nullable: false),
                    AggregatedTimeSpentInDays = table.Column<int>(type: "int", nullable: false),
                    AggregatedTimeEstimateInDays = table.Column<int>(type: "int", nullable: false),
                    IssueId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimatedAndSpentTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstimatedAndSpentTimes_Issue_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Release",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Released = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_EstimatedAndSpentTimes_IssueId",
                table: "EstimatedAndSpentTimes",
                column: "IssueId",
                unique: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstimatedAndSpentTimes");

            migrationBuilder.DropTable(
                name: "Release");

            migrationBuilder.DropTable(
                name: "Issue");

            migrationBuilder.DropTable(
                name: "IssueType");

            migrationBuilder.DropTable(
                name: "Parent");

            migrationBuilder.DropTable(
                name: "TeamBoard");
        }
    }
}
