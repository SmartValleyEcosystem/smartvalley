using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ProjectScoringCountersChangedToFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByAnalyst",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByHr",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByLawyer",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByTechnical",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"UPDATE dbo.Projects
SET IsScoredByAnalyst = IIF(AnalystEstimatesCount = 3, 1, 0), 
IsScoredByLawyer  = IIF(LawyerEstimatesCount = 3, 1, 0),
IsScoredByHr = IIF(HrEstimatesCount = 3, 1, 0),
IsScoredByTechnical = IIF(TechnicalEstimatesCount = 3, 1, 0)
");

            migrationBuilder.DropColumn(
                name: "AnalystEstimatesCount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "HrEstimatesCount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LawyerEstimatesCount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TechnicalEstimatesCount",
                table: "Projects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScoredByAnalyst",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsScoredByHr",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsScoredByLawyer",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsScoredByTechnical",
                table: "Projects");

            migrationBuilder.AddColumn<byte>(
                name: "AnalystEstimatesCount",
                table: "Projects",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "HrEstimatesCount",
                table: "Projects",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "LawyerEstimatesCount",
                table: "Projects",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "TechnicalEstimatesCount",
                table: "Projects",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
