using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ExpertiseApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpertiseArea",
                newName: "ExpertiseAreaType",
                table: "Questions");

            migrationBuilder.CreateTable(
                name: "ExpertApplications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicantId = table.Column<long>(type: "bigint", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    FacebookLink = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LinkedInLink = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Why = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpertApplications_Users_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpertiseAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertiseAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpertApplicationAreas",
                columns: table => new
                {
                    ExpertApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    ExpertiseAreaType = table.Column<int>(type: "int", nullable: false),
                    ExpertiseAreaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertApplicationAreas", x => new { x.ExpertApplicationId, x.ExpertiseAreaType });
                    table.ForeignKey(
                        name: "FK_ExpertApplicationAreas_ExpertApplications_ExpertApplicationId",
                        column: x => x.ExpertApplicationId,
                        principalTable: "ExpertApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpertApplicationAreas_ExpertiseAreas_ExpertiseAreaId",
                        column: x => x.ExpertiseAreaId,
                        principalTable: "ExpertiseAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpertApplicationAreas_ExpertiseAreaId",
                table: "ExpertApplicationAreas",
                column: "ExpertiseAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertApplications_ApplicantId",
                table: "ExpertApplications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertiseAreas_Name",
                table: "ExpertiseAreas",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpertApplicationAreas");

            migrationBuilder.DropTable(
                name: "ExpertApplications");

            migrationBuilder.DropTable(
                name: "ExpertiseAreas");

            migrationBuilder.RenameColumn(
                name: "ExpertiseAreaType",
                newName: "ExpertiseArea",
                table: "Questions");
        }
    }
}
