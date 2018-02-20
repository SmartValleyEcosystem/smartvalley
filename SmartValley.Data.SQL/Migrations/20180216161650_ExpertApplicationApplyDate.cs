using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ExpertApplicationApplyDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ApplyDate",
                table: "ExpertApplications",
                type: "datetimeoffset",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplyDate",
                table: "ExpertApplications");
        }
    }
}