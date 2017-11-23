using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ProjectDescriptionMerge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProblemDesc",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SolutionDesc",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "ProblemDesc",
                table: "Projects",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SolutionDesc",
                table: "Projects",
                maxLength: 255,
                nullable: true);
        }
    }
}
