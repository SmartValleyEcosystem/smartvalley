using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddedScoringEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OffersEndDate",
                table: "Scorings",
                newName: "OffersDueDate");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EstimatesDueDate",
                table: "Scorings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatesDueDate",
                table: "Scorings");

            migrationBuilder.RenameColumn(
                name: "OffersDueDate",
                table: "Scorings",
                newName: "OffersEndDate");
        }
    }
}
