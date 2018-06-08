using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AllotmentEventHasBeenAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllotmentEvents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 40, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TokenContractAddress = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTimeOffset>(nullable: true),
                    FinishDate = table.Column<DateTimeOffset>(nullable: false),
                    TotalTokens = table.Column<long>(nullable: false),
                    TokenTicker = table.Column<string>(maxLength: 6, nullable: false),
                    ProjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllotmentEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllotmentEvents_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllotmentEvents_ProjectId",
                table: "AllotmentEvents",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllotmentEvents");
        }
    }
}
