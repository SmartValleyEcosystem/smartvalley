using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ProjectAuthorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Projects",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.Sql(@"update Projects
                                    set AuthorId = u.Id
                                    from Users as u
                                    where Projects.AuthorAddress = u.Address;
                                  ");

            migrationBuilder.DropColumn(
                name: "AuthorAddress",
                table: "Projects");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AuthorId",
                table: "Projects",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_AuthorId",
                table: "Projects",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_AuthorId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_AuthorId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IcoDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "WhitePaperLink",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "AuthorAddress",
                table: "Projects",
                maxLength: 42,
                nullable: false,
                defaultValue: "");
        }
    }
}
