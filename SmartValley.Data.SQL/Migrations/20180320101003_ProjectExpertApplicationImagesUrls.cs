using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ProjectExpertApplicationImagesUrls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "CvName",
                table: "ExpertApplications");

            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "ExpertApplications");

            migrationBuilder.DropColumn(
                name: "ScanName",
                table: "ExpertApplications");

            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "ApplicationTeamMembers");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "ProjectTeamMembers",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Projects",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvUrl",
                table: "ExpertApplications",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "ExpertApplications",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScanUrl",
                table: "ExpertApplications",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "ApplicationTeamMembers",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CvUrl",
                table: "ExpertApplications");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "ExpertApplications");

            migrationBuilder.DropColumn(
                name: "ScanUrl",
                table: "ExpertApplications");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "ApplicationTeamMembers");

            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "ProjectTeamMembers",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvName",
                table: "ExpertApplications",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "ExpertApplications",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScanName",
                table: "ExpertApplications",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "ApplicationTeamMembers",
                maxLength: 50,
                nullable: true);
        }
    }
}
