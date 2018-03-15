using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class UpdateSocials : Migration
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSocialMedias",
                table: "ProjectSocialMedias",
                columns: new[] { "SocialMediaId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSocialMedias_SocialMediaId_ProjectId",
                table: "ProjectSocialMedias",
                columns: new[] { "SocialMediaId", "ProjectId" });

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

            migrationBuilder.AddColumn<int>(
                name: "SocialId",
                table: "ProjectSocialMedias",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSocialMedias",
                table: "ProjectSocialMedias",
                columns: new[] { "SocialId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSocialMedias_SocialId_ProjectId",
                table: "ProjectSocialMedias",
                columns: new[] { "SocialId", "ProjectId" });

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
