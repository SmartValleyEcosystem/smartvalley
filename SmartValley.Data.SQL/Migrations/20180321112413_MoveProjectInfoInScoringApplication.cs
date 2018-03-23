using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class MoveProjectInfoInScoringApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "ScoringApplications");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Saved",
                table: "ScoringApplications",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "ScoringApplications",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Completed",
                table: "ScoringApplications",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<long>(
                name: "CountryId",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplications_CountryId",
                table: "ScoringApplications",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringApplications_Countries_CountryId",
                table: "ScoringApplications",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoringApplications_Countries_CountryId",
                table: "ScoringApplications");

            migrationBuilder.DropIndex(
                name: "IX_ScoringApplications_CountryId",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "ScoringApplications");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Saved",
                table: "ScoringApplications",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "ScoringApplications",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Completed",
                table: "ScoringApplications",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ScoringApplications",
                nullable: true);
        }
    }
}
