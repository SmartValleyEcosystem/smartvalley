using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddStaticInformationToProjectScoringApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BitcointalkLink",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookLink",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubLink",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ICODate",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInLink",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediumLink",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectArea",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectDescription",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedditLink",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Site",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramLink",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterLink",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhitePaper",
                table: "ScoringApplications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BitcointalkLink",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "FacebookLink",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "GitHubLink",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "ICODate",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "LinkedInLink",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "MediumLink",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "ProjectArea",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "ProjectDescription",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "RedditLink",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "Site",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "TelegramLink",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "TwitterLink",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "WhitePaper",
                table: "ScoringApplications");
        }
    }
}
