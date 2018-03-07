using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddDatesForScoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreationDate",
                table: "Scorings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EstimatesEndDate",
                table: "Scorings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "OffersEndDate",
                table: "Scorings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartScoringDate",
                table: "Scorings",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Scorings");

            migrationBuilder.DropColumn(
                name: "EstimatesEndDate",
                table: "Scorings");

            migrationBuilder.DropColumn(
                name: "OffersEndDate",
                table: "Scorings");

            migrationBuilder.DropColumn(
                name: "StartScoringDate",
                table: "Scorings");
        }
    }
}
