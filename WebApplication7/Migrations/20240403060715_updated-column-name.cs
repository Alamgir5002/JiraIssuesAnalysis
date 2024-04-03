using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueAnalysisExtended.Migrations
{
    /// <inheritdoc />
    public partial class updatedcolumnname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "AggregatedTimeSpentInDays",
                table: "EstimatedAndSpentTimes",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "AggregatedTimeEstimateInDays",
                table: "EstimatedAndSpentTimes",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AggregatedTimeSpentInDays",
                table: "EstimatedAndSpentTimes",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "AggregatedTimeEstimateInDays",
                table: "EstimatedAndSpentTimes",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
