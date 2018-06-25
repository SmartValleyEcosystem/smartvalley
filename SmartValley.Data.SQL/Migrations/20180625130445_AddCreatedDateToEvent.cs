using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddCreatedDateToEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AllotmentEvents",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
            
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AllotmentEvents",
                nullable: false);
            
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreationDate",
                table: "Scorings",
                nullable: false);
            
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "AcceptingDeadline",
                table: "Scorings",
                nullable: false);
            
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ScoringDeadline",
                table: "Scorings",
                nullable: false);
            
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "EthereumTransactions",
                nullable: false);
            
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "ExpertApplications",
                nullable: false);
            
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ApplyDate",
                table: "ExpertApplications",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AllotmentEvents");
        }
    }
}
