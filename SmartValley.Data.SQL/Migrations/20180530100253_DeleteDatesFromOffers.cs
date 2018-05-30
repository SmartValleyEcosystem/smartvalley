using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class DeleteDatesFromOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OffersDueDate",
                table: "Scorings");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AcceptingDeadline",
                table: "Scorings",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.Sql(@"update Scorings
                                   set Scorings.AcceptingDeadline = offers.acceptingDeadline
                                   from 
                                   (select scoringId, max(ExpirationTimestamp) as acceptingDeadline
                                   from ScoringOffers
                                   group by scoringId) as offers
                                   where Scorings.Id = offers.scoringId");

            migrationBuilder.DropColumn(
                name: "ExpirationTimestamp",
                table: "ScoringOffers");

            migrationBuilder.RenameColumn(
                name: "EstimatesDueDate",
                table: "Scorings",
                newName: "ScoringDeadline");

            migrationBuilder.Sql(@"update Scorings
                                   set Scorings.ScoringDeadline = offers.scoringDeadline
                                   from 
                                   (select scoringId, max(EstimatesDueDate) as scoringDeadline
                                   from ScoringOffers
                                   group by scoringId) as offers
                                   where Scorings.Id = offers.scoringId");

            migrationBuilder.DropColumn(
                name: "EstimatesDueDate",
                table: "ScoringOffers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EstimatesDueDate",
                table: "ScoringOffers",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "ScoringDeadline",
                table: "Scorings",
                newName: "EstimatesDueDate");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpirationTimestamp",
                table: "ScoringOffers",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
            
            migrationBuilder.DropColumn(
                name: "AcceptingDeadline",
                table: "Scorings");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "OffersDueDate",
                table: "Scorings",
                nullable: true);
        }
    }
}
