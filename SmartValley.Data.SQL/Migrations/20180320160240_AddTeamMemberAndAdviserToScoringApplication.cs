using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddTeamMemberAndAdviserToScoringApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoringApplicationAnswers_ScoringApplications_ScoringApplicationId",
                table: "ScoringApplicationAnswers");

            migrationBuilder.AlterColumn<long>(
                name: "ScoringApplicationId",
                table: "ScoringApplicationAnswers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ScoringApplicationAdvisers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(nullable: true),
                    About = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    FacebookLink = table.Column<string>(nullable: true),
                    LinkedInLink = table.Column<string>(nullable: true),
                    ScoringApplicationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringApplicationAdvisers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoringApplicationAdvisers_ScoringApplications_ScoringApplicationId",
                        column: x => x.ScoringApplicationId,
                        principalTable: "ScoringApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoringApplicationTeamMembers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(nullable: true),
                    ProjectRole = table.Column<string>(nullable: true),
                    About = table.Column<string>(nullable: true),
                    FacebookLink = table.Column<string>(nullable: true),
                    LinkedInLink = table.Column<string>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true),
                    ScoringApplicationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringApplicationTeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoringApplicationTeamMembers_ScoringApplications_ScoringApplicationId",
                        column: x => x.ScoringApplicationId,
                        principalTable: "ScoringApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplicationAdvisers_ScoringApplicationId",
                table: "ScoringApplicationAdvisers",
                column: "ScoringApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplicationTeamMembers_ScoringApplicationId",
                table: "ScoringApplicationTeamMembers",
                column: "ScoringApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringApplicationAnswers_ScoringApplications_ScoringApplicationId",
                table: "ScoringApplicationAnswers",
                column: "ScoringApplicationId",
                principalTable: "ScoringApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoringApplicationAnswers_ScoringApplications_ScoringApplicationId",
                table: "ScoringApplicationAnswers");

            migrationBuilder.DropTable(
                name: "ScoringApplicationAdvisers");

            migrationBuilder.DropTable(
                name: "ScoringApplicationTeamMembers");

            migrationBuilder.AlterColumn<long>(
                name: "ScoringApplicationId",
                table: "ScoringApplicationAnswers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringApplicationAnswers_ScoringApplications_ScoringApplicationId",
                table: "ScoringApplicationAnswers",
                column: "ScoringApplicationId",
                principalTable: "ScoringApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
