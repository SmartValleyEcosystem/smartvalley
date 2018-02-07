using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddProjectExternalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ExternalId",
                table: "Projects",
                column: "ExternalId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_ExternalId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Projects");
        }
    }
}
