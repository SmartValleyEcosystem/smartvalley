using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddBigNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Share",
                table: "AllotmentEventParticipants",
                nullable: false,
                defaultValue: "0",
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "Bid",
                table: "AllotmentEventParticipants",
                nullable: false,
                defaultValue: "0",
                oldClrType: typeof(long));
            
            migrationBuilder.AlterColumn<string>(
                name: "Share",
                table: "AllotmentEventParticipants",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "Bid",
                table: "AllotmentEventParticipants",
                nullable: false,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Share",
                table: "AllotmentEventParticipants",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Bid",
                table: "AllotmentEventParticipants",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
