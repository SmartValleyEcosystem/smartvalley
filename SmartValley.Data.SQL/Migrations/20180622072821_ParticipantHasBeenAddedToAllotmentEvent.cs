using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ParticipantHasBeenAddedToAllotmentEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllotmentEventParticipants",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bid = table.Column<long>(nullable: false),
                    Share = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    AllotmentEventId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllotmentEventParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllotmentEventParticipants_AllotmentEvents_AllotmentEventId",
                        column: x => x.AllotmentEventId,
                        principalTable: "AllotmentEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllotmentEventParticipants_AllotmentEventId",
                table: "AllotmentEventParticipants",
                column: "AllotmentEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllotmentEventParticipants");
        }
    }
}
