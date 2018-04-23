using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class EstimateCommentHasBeenLinkedToConclusion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpertScoringConclusions",
                table: "ExpertScoringConclusions");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ExpertScoringConclusions",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<long>(
                name: "ExpertScoringConclusionId",
                table: "EstimateComments",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpertScoringConclusions",
                table: "ExpertScoringConclusions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertScoringConclusions_ScoringId",
                table: "ExpertScoringConclusions",
                column: "ScoringId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimateComments_ExpertScoringConclusionId",
                table: "EstimateComments",
                column: "ExpertScoringConclusionId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_ExpertScoringConclusions_ExpertScoringConclusionId",
                table: "EstimateComments",
                column: "ExpertScoringConclusionId",
                principalTable: "ExpertScoringConclusions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
                                    update c
                                    set c.ExpertScoringConclusionId = cn.Id
                                    from [dbo].[EstimateComments] c
                                    inner join [dbo].[ExpertScoringConclusions] cn on c.ScoringId = cn.ScoringId and c.ExpertId = cn.ExpertId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_ExpertScoringConclusions_ExpertScoringConclusionId",
                table: "EstimateComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpertScoringConclusions",
                table: "ExpertScoringConclusions");

            migrationBuilder.DropIndex(
                name: "IX_ExpertScoringConclusions_ScoringId",
                table: "ExpertScoringConclusions");

            migrationBuilder.DropIndex(
                name: "IX_EstimateComments_ExpertScoringConclusionId",
                table: "EstimateComments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ExpertScoringConclusions");

            migrationBuilder.DropColumn(
                name: "ExpertScoringConclusionId",
                table: "EstimateComments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpertScoringConclusions",
                table: "ExpertScoringConclusions",
                columns: new[] { "ScoringId", "Area", "ExpertId" });
        }
    }
}
