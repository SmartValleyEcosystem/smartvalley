using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ProjectStageIdRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageId",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "Projects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.DropColumn(
                name: "SocialNetwork_BitcoinTalk",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Github",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Facebook",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Medium",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Reddit",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Telegram",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Twitter",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Linkedin",
                table: "ApplicationTeamMembers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stage",
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

            migrationBuilder.AddColumn<int>(
                name: "StageId",
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
        }
    }
}
