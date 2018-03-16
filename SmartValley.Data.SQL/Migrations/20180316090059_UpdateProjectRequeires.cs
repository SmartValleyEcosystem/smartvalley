using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class UpdateProjectRequeires : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Projects",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "IcoDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Projects",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhitePaperLink",
                table: "Projects",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
