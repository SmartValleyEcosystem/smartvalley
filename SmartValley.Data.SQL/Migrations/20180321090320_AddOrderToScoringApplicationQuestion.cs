using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddOrderToScoringApplicationQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Filled",
                table: "ScoringApplications",
                newName: "Completed");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "ScoringApplicationQuestions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GroupOrder",
                table: "ScoringApplicationQuestions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupKey",
                table: "ScoringApplicationQuestions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ScoringApplicationQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplicationQuestions_Key",
                table: "ScoringApplicationQuestions",
                column: "Key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScoringApplicationQuestions_Key",
                table: "ScoringApplicationQuestions");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "ScoringApplicationQuestions");

            migrationBuilder.RenameColumn(
                name: "Completed",
                table: "ScoringApplications",
                newName: "Filled");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "ScoringApplicationQuestions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "GroupOrder",
                table: "ScoringApplicationQuestions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "GroupKey",
                table: "ScoringApplicationQuestions",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
