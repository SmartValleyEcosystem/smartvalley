using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class RenamedScoringDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatesEndDate",
                table: "Scorings");

            migrationBuilder.DropColumn(
                name: "StartScoringDate",
                table: "Scorings");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OffersEndDate",
                table: "Scorings",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ScoringEndDate",
                table: "Scorings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ScoringStartDate",
                table: "Scorings",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoringEndDate",
                table: "Scorings");

            migrationBuilder.DropColumn(
                name: "ScoringStartDate",
                table: "Scorings");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OffersEndDate",
                table: "Scorings",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EstimatesEndDate",
                table: "Scorings",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartScoringDate",
                table: "Scorings",
                nullable: true);
        }
    }
}
