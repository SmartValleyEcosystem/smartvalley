using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class RemovedObsoleteApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationTeamMembers");

            migrationBuilder.DropTable(
                name: "Applications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlockchainType = table.Column<string>(maxLength: 100, nullable: true),
                    FinancialModelLink = table.Column<string>(maxLength: 400, nullable: true),
                    HardCap = table.Column<string>(maxLength: 40, nullable: true),
                    InvestmentsAreAttracted = table.Column<bool>(nullable: false),
                    MvpLink = table.Column<string>(maxLength: 400, nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    ProjectStatus = table.Column<string>(maxLength: 100, nullable: true),
                    SoftCap = table.Column<string>(maxLength: 40, nullable: true),
                    WhitePaperLink = table.Column<string>(maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTeamMembers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    About = table.Column<string>(maxLength: 500, nullable: true),
                    ApplicationId = table.Column<long>(nullable: false),
                    Facebook = table.Column<string>(maxLength: 500, nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: false),
                    Linkedin = table.Column<string>(maxLength: 500, nullable: true),
                    PhotoUrl = table.Column<string>(maxLength: 200, nullable: true),
                    Role = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationTeamMembers_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ProjectId",
                table: "Applications",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTeamMembers_ApplicationId",
                table: "ApplicationTeamMembers",
                column: "ApplicationId");
        }
    }
}
