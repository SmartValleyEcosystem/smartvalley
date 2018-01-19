using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddScoringCounters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "AnalystEstimatesCount",
                table: "Projects",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "HrEstimatesCount",
                table: "Projects",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "LawyerEstimatesCount",
                table: "Projects",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "Projects",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "TechnicalEstimatesCount",
                table: "Projects",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalystEstimatesCount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "HrEstimatesCount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LawyerEstimatesCount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TechnicalEstimatesCount",
                table: "Projects");
        }
    }
}
