using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ScoringApplicationProjectIdHasBeenMadeUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScoringApplications_ProjectId",
                table: "ScoringApplications");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplications_ProjectId",
                table: "ScoringApplications",
                column: "ProjectId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScoringApplications_ProjectId",
                table: "ScoringApplications");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplications_ProjectId",
                table: "ScoringApplications",
                column: "ProjectId");
        }
    }
}
