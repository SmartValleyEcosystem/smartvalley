using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class EstimateCommentAddedNonNullableLinkToConclusion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_ExpertScoringConclusions_ExpertScoringConclusionId",
                table: "EstimateComments");

            migrationBuilder.AlterColumn<long>(
                name: "ExpertScoringConclusionId",
                table: "EstimateComments",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_ExpertScoringConclusions_ExpertScoringConclusionId",
                table: "EstimateComments",
                column: "ExpertScoringConclusionId",
                principalTable: "ExpertScoringConclusions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_ExpertScoringConclusions_ExpertScoringConclusionId",
                table: "EstimateComments");

            migrationBuilder.AlterColumn<long>(
                name: "ExpertScoringConclusionId",
                table: "EstimateComments",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_ExpertScoringConclusions_ExpertScoringConclusionId",
                table: "EstimateComments",
                column: "ExpertScoringConclusionId",
                principalTable: "ExpertScoringConclusions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
