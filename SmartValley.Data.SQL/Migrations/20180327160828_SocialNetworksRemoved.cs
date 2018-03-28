using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class SocialNetworksRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocialNetworks_BitcoinTalk",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Github",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Medium",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Reddit",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Telegram",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetworks_Twitter",
                table: "ProjectTeamMembers");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Linkedin",
                table: "ProjectTeamMembers",
                newName: "Linkedin");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Facebook",
                table: "ProjectTeamMembers",
                newName: "Facebook");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Twitter",
                table: "Projects",
                newName: "Twitter");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Telegram",
                table: "Projects",
                newName: "Telegram");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Reddit",
                table: "Projects",
                newName: "Reddit");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Medium",
                table: "Projects",
                newName: "Medium");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Linkedin",
                table: "Projects",
                newName: "Linkedin");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Github",
                table: "Projects",
                newName: "Github");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Facebook",
                table: "Projects",
                newName: "Facebook");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_BitcoinTalk",
                table: "Projects",
                newName: "BitcoinTalk");

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Linkedin",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "Linkedin",
                table: "ApplicationTeamMembers");

            migrationBuilder.RenameColumn(
                name: "Linkedin",
                table: "ProjectTeamMembers",
                newName: "SocialNetworks_Linkedin");

            migrationBuilder.RenameColumn(
                name: "Facebook",
                table: "ProjectTeamMembers",
                newName: "SocialNetworks_Facebook");

            migrationBuilder.RenameColumn(
                name: "Twitter",
                table: "Projects",
                newName: "SocialNetworks_Twitter");

            migrationBuilder.RenameColumn(
                name: "Telegram",
                table: "Projects",
                newName: "SocialNetworks_Telegram");

            migrationBuilder.RenameColumn(
                name: "Reddit",
                table: "Projects",
                newName: "SocialNetworks_Reddit");

            migrationBuilder.RenameColumn(
                name: "Medium",
                table: "Projects",
                newName: "SocialNetworks_Medium");

            migrationBuilder.RenameColumn(
                name: "Linkedin",
                table: "Projects",
                newName: "SocialNetworks_Linkedin");

            migrationBuilder.RenameColumn(
                name: "Github",
                table: "Projects",
                newName: "SocialNetworks_Github");

            migrationBuilder.RenameColumn(
                name: "Facebook",
                table: "Projects",
                newName: "SocialNetworks_Facebook");

            migrationBuilder.RenameColumn(
                name: "BitcoinTalk",
                table: "Projects",
                newName: "SocialNetworks_BitcoinTalk");

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_BitcoinTalk",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Github",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Medium",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Reddit",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Telegram",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Twitter",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

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
