using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ChangeTypeForScoringApplicationCategoryAndStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ScoringApplications");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "IcoDate",
                table: "ScoringApplications",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "ScoringApplications",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "ScoringApplications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stage",
                table: "ScoringApplications");

            migrationBuilder.AlterColumn<string>(
                name: "IcoDate",
                table: "ScoringApplications",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "ScoringApplications",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ScoringApplications",
                nullable: true);
        }
    }
}
