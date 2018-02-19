using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class Expert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpertApplicationAreas_ExpertiseAreas_ExpertiseAreaId",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropTable(
                name: "ExpertiseAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpertApplicationAreas",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropIndex(
                name: "IX_ExpertApplicationAreas_ExpertiseAreaId",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropColumn(
                name: "ExpertiseAreaType",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ExpertiseAreaType",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropColumn(
                name: "ExpertiseAreaId",
                table: "ExpertApplicationAreas");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AreaType",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "ExpertApplicationAreas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpertApplicationAreas",
                table: "ExpertApplicationAreas",
                columns: new[] { "ExpertApplicationId", "AreaId" });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Experts",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experts", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Experts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpertAreas",
                columns: table => new
                {
                    ExpertId = table.Column<long>(type: "bigint", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertAreas", x => new { x.ExpertId, x.AreaId });
                    table.ForeignKey(
                        name: "FK_ExpertAreas_Experts_ExpertId",
                        column: x => x.ExpertId,
                        principalTable: "Experts",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpertApplicationAreas_AreaId",
                table: "ExpertApplicationAreas",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_Name",
                table: "Areas",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertApplicationAreas_Areas_AreaId",
                table: "ExpertApplicationAreas",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpertApplicationAreas_Areas_AreaId",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "ExpertAreas");

            migrationBuilder.DropTable(
                name: "Experts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpertApplicationAreas",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropIndex(
                name: "IX_ExpertApplicationAreas_AreaId",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropColumn(
                name: "About",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AreaType",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "ExpertApplicationAreas");

            migrationBuilder.AddColumn<int>(
                name: "ExpertiseAreaType",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpertiseAreaType",
                table: "ExpertApplicationAreas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpertiseAreaId",
                table: "ExpertApplicationAreas",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpertApplicationAreas",
                table: "ExpertApplicationAreas",
                columns: new[] { "ExpertApplicationId", "ExpertiseAreaType" });

            migrationBuilder.CreateTable(
                name: "ExpertiseAreas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertiseAreas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpertApplicationAreas_ExpertiseAreaId",
                table: "ExpertApplicationAreas",
                column: "ExpertiseAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertiseAreas_Name",
                table: "ExpertiseAreas",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertApplicationAreas_ExpertiseAreas_ExpertiseAreaId",
                table: "ExpertApplicationAreas",
                column: "ExpertiseAreaId",
                principalTable: "ExpertiseAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
