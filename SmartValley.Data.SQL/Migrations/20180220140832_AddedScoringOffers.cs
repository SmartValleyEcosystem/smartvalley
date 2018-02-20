using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddedScoringOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoringOffers",
                columns: table => new
                {
                    ScoringId = table.Column<long>(type: "bigint", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    ExpertId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringOffers", x => new { x.ScoringId, x.AreaId, x.ExpertId });
                    table.ForeignKey(
                        name: "FK_ScoringOffers_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoringOffers_Experts_ExpertId",
                        column: x => x.ExpertId,
                        principalTable: "Experts",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoringOffers_Scorings_ScoringId",
                        column: x => x.ScoringId,
                        principalTable: "Scorings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoringOffers_AreaId",
                table: "ScoringOffers",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringOffers_ExpertId",
                table: "ScoringOffers",
                column: "ExpertId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringOffers_ScoringId_AreaId_ExpertId",
                table: "ScoringOffers",
                columns: new[] { "ScoringId", "AreaId", "ExpertId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoringOffers");
        }
    }
}
