using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class SocialNetworksRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Twitter",
                table: "ProjectTeamMembers",
                newName: "SocialNetworks_Twitter");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Telegram",
                table: "ProjectTeamMembers",
                newName: "SocialNetworks_Telegram");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Reddit",
                table: "ProjectTeamMembers",
                newName: "SocialNetworks_Reddit");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Medium",
                table: "ProjectTeamMembers",
                newName: "SocialNetworks_Medium");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Linkedin",
                table: "ProjectTeamMembers",
                newName: "SocialNetworks_Linkedin");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Github",
                table: "ProjectTeamMembers",
                newName: "SocialNetworks_Github");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Facebook",
                table: "ProjectTeamMembers",
                newName: "SocialNetworks_Facebook");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_BitcoinTalk",
                table: "ProjectTeamMembers",
                newName: "SocialNetworks_BitcoinTalk");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Twitter",
                table: "Projects",
                newName: "SocialNetworks_Twitter");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Telegram",
                table: "Projects",
                newName: "SocialNetworks_Telegram");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Reddit",
                table: "Projects",
                newName: "SocialNetworks_Reddit");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Medium",
                table: "Projects",
                newName: "SocialNetworks_Medium");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Linkedin",
                table: "Projects",
                newName: "SocialNetworks_Linkedin");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Github",
                table: "Projects",
                newName: "SocialNetworks_Github");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_Facebook",
                table: "Projects",
                newName: "SocialNetworks_Facebook");

            migrationBuilder.RenameColumn(
                name: "SocialNetwork_BitcoinTalk",
                table: "Projects",
                newName: "SocialNetworks_BitcoinTalk");

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_BitcoinTalk",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Facebook",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Github",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Linkedin",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Medium",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Reddit",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Telegram",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworks_Twitter",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "SocialNetworks_Twitter",
                table: "ProjectTeamMembers",
                newName: "SocialNetwork_Twitter");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Telegram",
                table: "ProjectTeamMembers",
                newName: "SocialNetwork_Telegram");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Reddit",
                table: "ProjectTeamMembers",
                newName: "SocialNetwork_Reddit");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Medium",
                table: "ProjectTeamMembers",
                newName: "SocialNetwork_Medium");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Linkedin",
                table: "ProjectTeamMembers",
                newName: "SocialNetwork_Linkedin");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Github",
                table: "ProjectTeamMembers",
                newName: "SocialNetwork_Github");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Facebook",
                table: "ProjectTeamMembers",
                newName: "SocialNetwork_Facebook");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_BitcoinTalk",
                table: "ProjectTeamMembers",
                newName: "SocialNetwork_BitcoinTalk");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Twitter",
                table: "Projects",
                newName: "SocialNetwork_Twitter");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Telegram",
                table: "Projects",
                newName: "SocialNetwork_Telegram");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Reddit",
                table: "Projects",
                newName: "SocialNetwork_Reddit");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Medium",
                table: "Projects",
                newName: "SocialNetwork_Medium");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Linkedin",
                table: "Projects",
                newName: "SocialNetwork_Linkedin");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Github",
                table: "Projects",
                newName: "SocialNetwork_Github");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_Facebook",
                table: "Projects",
                newName: "SocialNetwork_Facebook");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks_BitcoinTalk",
                table: "Projects",
                newName: "SocialNetwork_BitcoinTalk");

            migrationBuilder.CreateTable(
                name: "ApplicationTeamMembers_SocialNetwork",
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
                    table.PrimaryKey("PK_ApplicationTeamMembers_SocialNetwork", x => x.ApplicationTeamMemberId);
                    table.ForeignKey(
                        name: "FK_ApplicationTeamMembers_SocialNetwork_ApplicationTeamMembers_ApplicationTeamMemberId",
                        column: x => x.ApplicationTeamMemberId,
                        principalTable: "ApplicationTeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
