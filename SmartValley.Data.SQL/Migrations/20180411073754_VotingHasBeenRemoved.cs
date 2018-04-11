using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class VotingHasBeenRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VotingProjects");

            migrationBuilder.DropTable(
                name: "Votings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Votings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EndDate = table.Column<DateTimeOffset>(nullable: false),
                    StartDate = table.Column<DateTimeOffset>(nullable: false),
                    VotingAddress = table.Column<string>(maxLength: 42, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VotingProjects",
                columns: table => new
                {
                    ProjectId = table.Column<long>(nullable: false),
                    VotingId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingProjects", x => new { x.ProjectId, x.VotingId });
                    table.ForeignKey(
                        name: "FK_VotingProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VotingProjects_Votings_VotingId",
                        column: x => x.VotingId,
                        principalTable: "Votings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VotingProjects_VotingId",
                table: "VotingProjects",
                column: "VotingId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingProjects_ProjectId_VotingId",
                table: "VotingProjects",
                columns: new[] { "ProjectId", "VotingId" });
        }
    }
}
