using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ScoringApplicationQuestionParentIdHasBeenMadeLongNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Completed",
                table: "ScoringApplications",
                newName: "Submitted");

            migrationBuilder.AlterColumn<long>(
                name: "ParentId",
                table: "ScoringApplicationQuestions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Submitted",
                table: "ScoringApplications",
                newName: "Completed");

            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                table: "ScoringApplicationQuestions",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
