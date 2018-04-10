using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ScoringOffersAndConslusionsHasBeenAddedToScoringAggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Scorings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("update [dbo].[Scorings] " +
                                 "set Status = case " +
                                    "when score is null then 1 " +
                                    "else 2 " +
                                 "end");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Scorings");
        }
    }
}
