using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class EstimateCommentAndOfferStructurerReorganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_Experts_ExpertId",
                table: "EstimateComments");

            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_Scorings_ScoringId",
                table: "EstimateComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoringOffers_Areas_AreaId",
                table: "ScoringOffers");

            migrationBuilder.DropIndex(
                name: "IX_ScoringOffers_AreaId",
                table: "ScoringOffers");

            migrationBuilder.DropIndex(
                name: "IX_EstimateComments_ExpertId",
                table: "EstimateComments");

            migrationBuilder.DropIndex(
                name: "IX_EstimateComments_ScoringId",
                table: "EstimateComments");

            migrationBuilder.DropColumn(
                name: "ExpertId",
                table: "EstimateComments");

            migrationBuilder.DropColumn(
                name: "ScoringId",
                table: "EstimateComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ExpertId",
                table: "EstimateComments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ScoringId",
                table: "EstimateComments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ScoringOffers_AreaId",
                table: "ScoringOffers",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimateComments_ExpertId",
                table: "EstimateComments",
                column: "ExpertId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimateComments_ScoringId",
                table: "EstimateComments",
                column: "ScoringId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_Experts_ExpertId",
                table: "EstimateComments",
                column: "ExpertId",
                principalTable: "Experts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_Scorings_ScoringId",
                table: "EstimateComments",
                column: "ScoringId",
                principalTable: "Scorings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringOffers_Areas_AreaId",
                table: "ScoringOffers",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
