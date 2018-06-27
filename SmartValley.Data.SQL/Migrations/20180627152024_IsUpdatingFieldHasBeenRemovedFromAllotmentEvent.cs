using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class IsUpdatingFieldHasBeenRemovedFromAllotmentEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUpdating",
                table: "AllotmentEvents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUpdating",
                table: "AllotmentEvents",
                nullable: false,
                defaultValue: false);
        }
    }
}