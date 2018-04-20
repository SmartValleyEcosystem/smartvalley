using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AreaScoringUntiedFromArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AreaScorings_Areas_AreaId",
                table: "AreaScorings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AreaScorings_AreaId",
                table: "AreaScorings",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AreaScorings_Areas_AreaId",
                table: "AreaScorings",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
