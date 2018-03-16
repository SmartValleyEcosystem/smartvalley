using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class UpdateProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSocialMedias_SocialMedias_SocialId",
                table: "ProjectSocialMedias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSocialMedias",
                table: "ProjectSocialMedias");

            migrationBuilder.DropIndex(
                name: "IX_ProjectSocialMedias_SocialId_ProjectId",
                table: "ProjectSocialMedias");

            migrationBuilder.DropColumn(
                name: "SocialId",
                table: "ProjectSocialMedias");

            migrationBuilder.AddColumn<int>(
                name: "SocialMediaId",
                table: "ProjectSocialMedias",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Projects",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "IcoDate",
                table: "Projects",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Projects",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WhitePaperLink",
                table: "Projects",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSocialMedias",
                table: "ProjectSocialMedias",
                columns: new[] {"SocialMediaId", "ProjectId"});

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSocialMedias_SocialMediaId_ProjectId",
                table: "ProjectSocialMedias",
                columns: new[] {"SocialMediaId", "ProjectId"});

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSocialMedias_SocialMedias_SocialMediaId",
                table: "ProjectSocialMedias",
                column: "SocialMediaId",
                principalTable: "SocialMedias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSocialMedias_SocialMedias_SocialMediaId",
                table: "ProjectSocialMedias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSocialMedias",
                table: "ProjectSocialMedias");

            migrationBuilder.DropIndex(
                name: "IX_ProjectSocialMedias_SocialMediaId_ProjectId",
                table: "ProjectSocialMedias");

            migrationBuilder.DropColumn(
                name: "SocialMediaId",
                table: "ProjectSocialMedias");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IcoDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "WhitePaperLink",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "SocialId",
                table: "ProjectSocialMedias",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSocialMedias",
                table: "ProjectSocialMedias",
                columns: new[] {"SocialId", "ProjectId"});

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSocialMedias_SocialId_ProjectId",
                table: "ProjectSocialMedias",
                columns: new[] {"SocialId", "ProjectId"});

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSocialMedias_SocialMedias_SocialId",
                table: "ProjectSocialMedias",
                column: "SocialId",
                principalTable: "SocialMedias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}