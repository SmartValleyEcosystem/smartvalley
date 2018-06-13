using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class UpdateAllotmentEventDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTokens",
                table: "AllotmentEvents");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FinishDate",
                table: "AllotmentEvents",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AddColumn<int>(
                name: "TokenDecimals",
                table: "AllotmentEvents",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenDecimals",
                table: "AllotmentEvents");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FinishDate",
                table: "AllotmentEvents",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TotalTokens",
                table: "AllotmentEvents",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
