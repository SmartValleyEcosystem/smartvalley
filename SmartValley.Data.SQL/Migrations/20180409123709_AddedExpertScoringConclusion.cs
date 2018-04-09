using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddedExpertScoringConclusion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpertScoringConclusions",
                columns: table => new
                {
                    ExpertId = table.Column<long>(nullable: false),
                    ScoringId = table.Column<long>(nullable: false),
                    Area = table.Column<int>(nullable: false),
                    Conclusion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertScoringConclusions", x => new { x.ScoringId, x.Area, x.ExpertId });
                    table.ForeignKey(
                        name: "FK_ExpertScoringConclusions_Experts_ExpertId",
                        column: x => x.ExpertId,
                        principalTable: "Experts",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpertScoringConclusions_Scorings_ScoringId",
                        column: x => x.ScoringId,
                        principalTable: "Scorings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpertScoringConclusions_ExpertId",
                table: "ExpertScoringConclusions",
                column: "ExpertId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpertScoringConclusions");
        }
    }
}
