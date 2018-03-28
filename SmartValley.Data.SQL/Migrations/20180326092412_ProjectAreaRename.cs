using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ProjectAreaRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Categories_CategoryId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Stages_StageId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_StageId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ProjectArea",
                table: "ScoringApplications",
                newName: "Category");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Projects",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_BitcoinTalk",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Facebook",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Github",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Linkedin",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Medium",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Reddit",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Telegram",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Twitter",
                table: "ApplicationTeamMembers");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "ScoringApplications",
                newName: "ProjectArea");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Projects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ApplicationTeamMembers_SocialNetworks",
                columns: table => new
                {
                    ApplicationTeamMemberId = table.Column<long>(nullable: false),
                    BitcoinTalk = table.Column<string>(maxLength: 500, nullable: true),
                    Facebook = table.Column<string>(maxLength: 500, nullable: true),
                    Github = table.Column<string>(maxLength: 500, nullable: true),
                    Linkedin = table.Column<string>(maxLength: 500, nullable: true),
                    Medium = table.Column<string>(maxLength: 500, nullable: true),
                    Reddit = table.Column<string>(maxLength: 500, nullable: true),
                    Telegram = table.Column<string>(maxLength: 500, nullable: true),
                    Twitter = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTeamMembers_SocialNetworks", x => x.ApplicationTeamMemberId);
                    table.ForeignKey(
                        name: "FK_ApplicationTeamMembers_SocialNetworks_ApplicationTeamMembers_ApplicationTeamMemberId",
                        column: x => x.ApplicationTeamMemberId,
                        principalTable: "ApplicationTeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StageId",
                table: "Projects",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stages_Name",
                table: "Stages",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Categories_CategoryId",
                table: "Projects",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Stages_StageId",
                table: "Projects",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
