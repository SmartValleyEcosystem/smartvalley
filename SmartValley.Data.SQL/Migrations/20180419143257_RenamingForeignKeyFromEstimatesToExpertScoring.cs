using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class RenamingForeignKeyFromEstimatesToExpertScoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estimates_ExpertScorings_ExpertScoringConclusionId",
                table: "Estimates");

            migrationBuilder.RenameColumn(
                name: "ExpertScoringConclusionId",
                table: "Estimates",
                newName: "ExpertScoringId");

            migrationBuilder.RenameIndex(
                name: "IX_Estimates_ExpertScoringConclusionId",
                table: "Estimates",
                newName: "IX_Estimates_ExpertScoringId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estimates_ExpertScorings_ExpertScoringId",
                table: "Estimates",
                column: "ExpertScoringId",
                principalTable: "ExpertScorings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estimates_ExpertScorings_ExpertScoringId",
                table: "Estimates");

            migrationBuilder.RenameColumn(
                name: "ExpertScoringId",
                table: "Estimates",
                newName: "ExpertScoringConclusionId");

            migrationBuilder.RenameIndex(
                name: "IX_Estimates_ExpertScoringId",
                table: "Estimates",
                newName: "IX_Estimates_ExpertScoringConclusionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estimates_ExpertScorings_ExpertScoringConclusionId",
                table: "Estimates",
                column: "ExpertScoringConclusionId",
                principalTable: "ExpertScorings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
