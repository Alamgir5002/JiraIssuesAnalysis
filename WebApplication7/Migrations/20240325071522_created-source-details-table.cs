using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class createdsourcedetailstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SourceCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceUserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SourceAuthToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceCredentials", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SourceCredentials_SourceUserEmail",
                table: "SourceCredentials",
                column: "SourceUserEmail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SourceCredentials");
        }
    }
}
