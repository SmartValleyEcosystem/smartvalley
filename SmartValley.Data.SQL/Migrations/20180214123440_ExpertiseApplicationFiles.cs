using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ExpertiseApplicationFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvName",
                table: "ExpertApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "ExpertApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScanName",
                table: "ExpertApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvName",
                table: "ExpertApplications");

            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "ExpertApplications");

            migrationBuilder.DropColumn(
                name: "ScanName",
                table: "ExpertApplications");
        }
    }
}
