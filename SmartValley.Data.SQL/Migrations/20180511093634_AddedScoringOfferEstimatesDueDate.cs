using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddedScoringOfferEstimatesDueDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ExpirationTimestamp",
                table: "ScoringOffers",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EstimatesDueDate",
                table: "ScoringOffers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatesDueDate",
                table: "ScoringOffers");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ExpirationTimestamp",
                table: "ScoringOffers",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));
        }
    }
}
