using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ExpertApplicationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpertApplicationAreas_ExpertiseAreas_ExpertiseAreaId",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpertApplicationAreas",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropColumn(
                name: "ExpertiseAreaType",
                table: "ExpertApplicationAreas");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ExpertApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ExpertiseAreaId",
                table: "ExpertApplicationAreas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ExpertApplicationAreas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpertApplicationAreas",
                table: "ExpertApplicationAreas",
                columns: new[] { "ExpertApplicationId", "ExpertiseAreaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertApplicationAreas_ExpertiseAreas_ExpertiseAreaId",
                table: "ExpertApplicationAreas",
                column: "ExpertiseAreaId",
                principalTable: "ExpertiseAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpertApplicationAreas_ExpertiseAreas_ExpertiseAreaId",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpertApplicationAreas",
                table: "ExpertApplicationAreas");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ExpertApplications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ExpertApplicationAreas");

            migrationBuilder.AlterColumn<int>(
                name: "ExpertiseAreaId",
                table: "ExpertApplicationAreas",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ExpertiseAreaType",
                table: "ExpertApplicationAreas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpertApplicationAreas",
                table: "ExpertApplicationAreas",
                columns: new[] { "ExpertApplicationId", "ExpertiseAreaType" });

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
