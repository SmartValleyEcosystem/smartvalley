using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class SocialMediaRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationTeamMemberSocialMedias");

            migrationBuilder.DropTable(
                name: "ProjectSocialMedias");

            migrationBuilder.DropTable(
                name: "ProjectTeamMemberSocialMedias");

            migrationBuilder.DropTable(
                name: "SocialMedias");

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_BitcoinTalk",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Facebook",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Github",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Linkedin",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Medium",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Reddit",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Telegram",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Twitter",
                table: "ProjectTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_BitcoinTalk",
                table: "Projects",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Facebook",
                table: "Projects",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Github",
                table: "Projects",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Linkedin",
                table: "Projects",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Medium",
                table: "Projects",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Reddit",
                table: "Projects",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Telegram",
                table: "Projects",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Twitter",
                table: "Projects",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_BitcoinTalk",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Facebook",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Github",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Linkedin",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Medium",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Reddit",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Telegram",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork_Twitter",
                table: "ApplicationTeamMembers",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocialNetwork_BitcoinTalk",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Facebook",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Github",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Linkedin",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Medium",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Reddit",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Telegram",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Twitter",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_BitcoinTalk",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Facebook",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Github",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Linkedin",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Medium",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Reddit",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Telegram",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Twitter",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_BitcoinTalk",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Facebook",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Github",
                table: "ApplicationTeamMembers");

            migrationBuilder.DropColumn(
                name: "SocialNetwork_Linkedin",
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

            migrationBuilder.CreateTable(
                name: "SocialMedias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTeamMemberSocialMedias",
                columns: table => new
                {
                    SocialMediaId = table.Column<int>(nullable: false),
                    TeamMemberId = table.Column<long>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTeamMemberSocialMedias", x => new { x.SocialMediaId, x.TeamMemberId });
                    table.ForeignKey(
                        name: "FK_ApplicationTeamMemberSocialMedias_SocialMedias_SocialMediaId",
                        column: x => x.SocialMediaId,
                        principalTable: "SocialMedias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationTeamMemberSocialMedias_ProjectTeamMembers_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "ProjectTeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSocialMedias",
                columns: table => new
                {
                    SocialMediaId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSocialMedias", x => new { x.SocialMediaId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectSocialMedias_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectSocialMedias_SocialMedias_SocialMediaId",
                        column: x => x.SocialMediaId,
                        principalTable: "SocialMedias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTeamMemberSocialMedias",
                columns: table => new
                {
                    SocialMediaId = table.Column<int>(nullable: false),
                    TeamMemberId = table.Column<long>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeamMemberSocialMedias", x => new { x.SocialMediaId, x.TeamMemberId });
                    table.ForeignKey(
                        name: "FK_ProjectTeamMemberSocialMedias_SocialMedias_SocialMediaId",
                        column: x => x.SocialMediaId,
                        principalTable: "SocialMedias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTeamMemberSocialMedias_ProjectTeamMembers_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "ProjectTeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTeamMemberSocialMedias_TeamMemberId",
                table: "ApplicationTeamMemberSocialMedias",
                column: "TeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTeamMemberSocialMedias_SocialMediaId_TeamMemberId",
                table: "ApplicationTeamMemberSocialMedias",
                columns: new[] { "SocialMediaId", "TeamMemberId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSocialMedias_ProjectId",
                table: "ProjectSocialMedias",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSocialMedias_SocialMediaId_ProjectId",
                table: "ProjectSocialMedias",
                columns: new[] { "SocialMediaId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamMemberSocialMedias_TeamMemberId",
                table: "ProjectTeamMemberSocialMedias",
                column: "TeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamMemberSocialMedias_SocialMediaId_TeamMemberId",
                table: "ProjectTeamMemberSocialMedias",
                columns: new[] { "SocialMediaId", "TeamMemberId" });

            migrationBuilder.CreateIndex(
                name: "IX_SocialMedias_Name",
                table: "SocialMedias",
                column: "Name",
                unique: true);
        }
    }
}
