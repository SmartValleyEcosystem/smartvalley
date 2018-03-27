using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ScoringAlgorithmUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "AreaScorings",
                nullable: true);

            migrationBuilder.Sql(@"update AreaScorings
                                    set Score = 25
                                    where IsCompleted = 1;
                                  ");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "AreaScorings");

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "Areas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("update Areas set MaxScore = 25;");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "MinScore",
                table: "Questions",
                newName: "Weight");

            migrationBuilder.Sql("update Questions set Weight = 10;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "AreaScorings");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Questions",
                newName: "MinScore");

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "AreaScorings",
                nullable: false,
                defaultValue: false);
        }
    }
}
