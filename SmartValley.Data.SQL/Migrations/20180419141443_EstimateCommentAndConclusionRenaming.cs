using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class EstimateCommentAndConclusionRenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_ExpertScoringConclusions_ExpertScoringConclusionId",
                table: "EstimateComments");

            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_ScoringCriteria_ScoringCriterionId",
                table: "EstimateComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpertScoringConclusions_Experts_ExpertId",
                table: "ExpertScoringConclusions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpertScoringConclusions_Scorings_ScoringId",
                table: "ExpertScoringConclusions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpertScoringConclusions",
                table: "ExpertScoringConclusions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstimateComments",
                table: "EstimateComments");

            migrationBuilder.RenameTable(
                name: "ExpertScoringConclusions",
                newName: "ExpertScorings");

            migrationBuilder.RenameTable(
                name: "EstimateComments",
                newName: "Estimates");

            migrationBuilder.RenameIndex(
                name: "IX_ExpertScoringConclusions_ScoringId",
                table: "ExpertScorings",
                newName: "IX_ExpertScorings_ScoringId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpertScoringConclusions_ExpertId",
                table: "ExpertScorings",
                newName: "IX_ExpertScorings_ExpertId");

            migrationBuilder.RenameIndex(
                name: "IX_EstimateComments_ScoringCriterionId",
                table: "Estimates",
                newName: "IX_Estimates_ScoringCriterionId");

            migrationBuilder.RenameIndex(
                name: "IX_EstimateComments_ExpertScoringConclusionId",
                table: "Estimates",
                newName: "IX_Estimates_ExpertScoringConclusionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpertScorings",
                table: "ExpertScorings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estimates",
                table: "Estimates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Estimates_ExpertScorings_ExpertScoringConclusionId",
                table: "Estimates",
                column: "ExpertScoringConclusionId",
                principalTable: "ExpertScorings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estimates_ScoringCriteria_ScoringCriterionId",
                table: "Estimates",
                column: "ScoringCriterionId",
                principalTable: "ScoringCriteria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertScorings_Experts_ExpertId",
                table: "ExpertScorings",
                column: "ExpertId",
                principalTable: "Experts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertScorings_Scorings_ScoringId",
                table: "ExpertScorings",
                column: "ScoringId",
                principalTable: "Scorings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estimates_ExpertScorings_ExpertScoringConclusionId",
                table: "Estimates");

            migrationBuilder.DropForeignKey(
                name: "FK_Estimates_ScoringCriteria_ScoringCriterionId",
                table: "Estimates");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpertScorings_Experts_ExpertId",
                table: "ExpertScorings");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpertScorings_Scorings_ScoringId",
                table: "ExpertScorings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpertScorings",
                table: "ExpertScorings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estimates",
                table: "Estimates");

            migrationBuilder.RenameTable(
                name: "ExpertScorings",
                newName: "ExpertScoringConclusions");

            migrationBuilder.RenameTable(
                name: "Estimates",
                newName: "EstimateComments");

            migrationBuilder.RenameIndex(
                name: "IX_ExpertScorings_ScoringId",
                table: "ExpertScoringConclusions",
                newName: "IX_ExpertScoringConclusions_ScoringId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpertScorings_ExpertId",
                table: "ExpertScoringConclusions",
                newName: "IX_ExpertScoringConclusions_ExpertId");

            migrationBuilder.RenameIndex(
                name: "IX_Estimates_ScoringCriterionId",
                table: "EstimateComments",
                newName: "IX_EstimateComments_ScoringCriterionId");

            migrationBuilder.RenameIndex(
                name: "IX_Estimates_ExpertScoringConclusionId",
                table: "EstimateComments",
                newName: "IX_EstimateComments_ExpertScoringConclusionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpertScoringConclusions",
                table: "ExpertScoringConclusions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstimateComments",
                table: "EstimateComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_ExpertScoringConclusions_ExpertScoringConclusionId",
                table: "EstimateComments",
                column: "ExpertScoringConclusionId",
                principalTable: "ExpertScoringConclusions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_ScoringCriteria_ScoringCriterionId",
                table: "EstimateComments",
                column: "ScoringCriterionId",
                principalTable: "ScoringCriteria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertScoringConclusions_Experts_ExpertId",
                table: "ExpertScoringConclusions",
                column: "ExpertId",
                principalTable: "Experts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertScoringConclusions_Scorings_ScoringId",
                table: "ExpertScoringConclusions",
                column: "ScoringId",
                principalTable: "Scorings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
