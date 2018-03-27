using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class RenamedQuestionToScoringCriterion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_Questions_QuestionId",
                table: "EstimateComments");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "ScoringCriteria");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "EstimateComments",
                newName: "ScoringCriterionId");

            migrationBuilder.RenameIndex(
                name: "IX_EstimateComments_QuestionId",
                table: "EstimateComments",
                newName: "IX_EstimateComments_ScoringCriterionId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_ScoringCriteria_ScoringCriterionId",
                table: "EstimateComments",
                column: "ScoringCriterionId",
                principalTable: "ScoringCriteria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_ScoringCriteria_ScoringCriterionId",
                table: "EstimateComments");

            migrationBuilder.RenameTable(
                name: "ScoringCriteria",
                newName: "Questions");

            migrationBuilder.RenameColumn(
                name: "ScoringCriterionId",
                table: "EstimateComments",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_EstimateComments_ScoringCriterionId",
                table: "EstimateComments",
                newName: "IX_EstimateComments_QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_Questions_QuestionId",
                table: "EstimateComments",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}