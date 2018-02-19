using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddedAreaScorings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaScorings",
                columns: table => new
                {
                    ScoringId = table.Column<long>(type: "bigint", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    ExpertsCount = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaScorings", x => new { x.ScoringId, x.AreaId });
                    table.ForeignKey(
                        name: "FK_AreaScorings_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaScorings_Scorings_ScoringId",
                        column: x => x.ScoringId,
                        principalTable: "Scorings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaScorings_ScoringId_AreaId",
                table: "AreaScorings",
                columns: new[] { "ScoringId", "AreaId" });

            migrationBuilder.Sql(@"INSERT INTO Areas (Id, Name) VALUES
(1, 'Hr'),
(2, 'Analyst'),
(3, 'Tech'),
(4, 'Lawyer')");

            migrationBuilder.Sql(@"INSERT INTO AreaScorings (ScoringId, AreaId, ExpertsCount, IsCompleted)
(
SELECT Id, 1, 3, IIF (IsScoredByHr = 1, 1, 0)
FROM Scorings
UNION
SELECT Id, 2, 3, IIF (IsScoredByAnalyst = 1, 1, 0)
FROM Scorings
UNION
SELECT Id, 3, 3, IIF (IsScoredByTechnical = 1, 1, 0)
FROM Scorings
UNION
SELECT Id, 4, 3, IIF (IsScoredByLawyer = 1, 1, 0)
FROM Scorings
)");

            migrationBuilder.DropColumn(
                name: "IsScoredByAnalyst",
                table: "Scorings");

            migrationBuilder.DropColumn(
                name: "IsScoredByHr",
                table: "Scorings");

            migrationBuilder.DropColumn(
                name: "IsScoredByLawyer",
                table: "Scorings");

            migrationBuilder.DropColumn(
                name: "IsScoredByTechnical",
                table: "Scorings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaScorings");

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByAnalyst",
                table: "Scorings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByHr",
                table: "Scorings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByLawyer",
                table: "Scorings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByTechnical",
                table: "Scorings",
                nullable: false,
                defaultValue: false);
        }
    }
}
