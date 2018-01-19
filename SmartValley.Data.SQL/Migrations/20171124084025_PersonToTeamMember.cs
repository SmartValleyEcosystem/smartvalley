using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class PersonToTeamMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "PersonType",
                table: "Persons");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "TeamMembers");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "TeamMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamMembers",
                table: "TeamMembers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_ApplicationId",
                table: "TeamMembers",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Applications_ApplicationId",
                table: "TeamMembers",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Applications_ApplicationId",
                table: "TeamMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamMembers",
                table: "TeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_ApplicationId",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "TeamMembers");

            migrationBuilder.RenameTable(
                name: "TeamMembers",
                newName: "Persons");

            migrationBuilder.AddColumn<int>(
                name: "PersonType",
                table: "Persons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "Id");
        }
    }
}
