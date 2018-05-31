using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddBitcointalkLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "ExpertApplications");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Users",
                nullable: true);

            migrationBuilder.Sql(@"update Users
                                set Users.About = e.About
                                from Experts as e
								where Users.Id = e.UserId");

            migrationBuilder.DropColumn(
                name: "About",
                table: "Experts");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BitcointalkLink",
                table: "Users",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Users",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CountryId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookLink",
                table: "Users",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInLink",
                table: "Users",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sex",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BitcointalkLink",
                table: "ExpertApplications",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CountryId",
                table: "ExpertApplications",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.CreateIndex(
                name: "IX_ExpertApplications_CountryId",
                table: "ExpertApplications",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertApplications_Countries_CountryId",
                table: "ExpertApplications",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryId",
                table: "Users",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Countries_CountryId",
                table: "Users",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpertApplications_Countries_CountryId",
                table: "ExpertApplications");

            migrationBuilder.DropIndex(
                name: "IX_ExpertApplications_CountryId",
                table: "ExpertApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Countries_CountryId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CountryId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Experts",
                nullable: true);

            migrationBuilder.Sql(@"update Experts
                                set Experts.About = Users.About
                                from Users
								where Users.Id = Experts.UserId");

            migrationBuilder.DropColumn(
                name: "About",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BitcointalkLink",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FacebookLink",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LinkedInLink",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BitcointalkLink",
                table: "ExpertApplications");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "ExpertApplications");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ExpertApplications",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
