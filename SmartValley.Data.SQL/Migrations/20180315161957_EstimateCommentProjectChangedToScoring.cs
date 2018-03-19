using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class EstimateCommentProjectChangedToScoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_Projects_ProjectId",
                table: "EstimateComments");

            migrationBuilder.DropIndex(
                name: "IX_EstimateComments_ProjectId",
                table: "EstimateComments");

            migrationBuilder.AddColumn<long>(
                name: "ScoringId",
                table: "EstimateComments",
                nullable:true);

            migrationBuilder.Sql(@"update ec 
                                   set ec.ScoringId = s.Id
                                   from dbo.EstimateComments ec 
                                   inner join Scorings s on ec.ProjectId = s.ProjectId");

            migrationBuilder.AlterColumn<long>(
                name: "ScoringId",
                table: "EstimateComments",
                nullable: false);


            migrationBuilder.CreateIndex(
                name: "IX_EstimateComments_ScoringId",
                table: "EstimateComments",
                column: "ScoringId");


            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_Scorings_ScoringId",
                table: "EstimateComments",
                column: "ScoringId",
                principalTable: "Scorings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "EstimateComments"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_Scorings_ScoringId",
                table: "EstimateComments");

            migrationBuilder.RenameColumn(
                name: "ScoringId",
                table: "EstimateComments",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_EstimateComments_ScoringId",
                table: "EstimateComments",
                newName: "IX_EstimateComments_ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_Projects_ProjectId",
                table: "EstimateComments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
