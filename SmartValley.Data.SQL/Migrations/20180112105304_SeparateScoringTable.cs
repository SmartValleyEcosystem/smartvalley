using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class SeparateScoringTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scorings",
                columns: table => new
                                  {
                                      Id = table.Column<long>(type: "bigint", nullable: false)
                                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                                      ContractAddress = table.Column<string>(type: "nvarchar(42)", maxLength: 42, nullable: false),
                                      IsScoredByAnalyst = table.Column<bool>(type: "bit", nullable: false),
                                      IsScoredByHr = table.Column<bool>(type: "bit", nullable: false),
                                      IsScoredByLawyer = table.Column<bool>(type: "bit", nullable: false),
                                      IsScoredByTechnical = table.Column<bool>(type: "bit", nullable: false),
                                      ProjectId = table.Column<long>(type: "bigint", nullable: false),
                                      Score = table.Column<double>(type: "float", nullable: true)
                                  },
                constraints: table =>
                             {
                                 table.PrimaryKey("PK_Scorings", x => x.Id);
                                 table.ForeignKey(
                                     name: "FK_Scorings_Projects_ProjectId",
                                     column: x => x.ProjectId,
                                     principalTable: "Projects",
                                     principalColumn: "Id",
                                     onDelete: ReferentialAction.Cascade);
                             });

            migrationBuilder.Sql(@"INSERT INTO dbo.Scorings (ProjectId, IsScoredByAnalyst, IsScoredByHr, IsScoredByLawyer, IsScoredByTechnical, Score, ContractAddress)
SELECT Id, IsScoredByAnalyst, IsScoredByHr, IsScoredByLawyer, IsScoredByTechnical, Score, ProjectAddress
FROM dbo.Projects");

            migrationBuilder.CreateIndex(
                name: "IX_Scorings_ProjectId",
                table: "Scorings",
                column: "ProjectId",
                unique: true);

            migrationBuilder.DropColumn(
                name: "IsScoredByAnalyst",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsScoredByHr",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsScoredByLawyer",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsScoredByTechnical",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectAddress",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Projects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scorings");

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByAnalyst",
                table: "Projects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByHr",
                table: "Projects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByLawyer",
                table: "Projects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScoredByTechnical",
                table: "Projects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProjectAddress",
                table: "Projects",
                maxLength: 42,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "Projects",
                nullable: true);
        }
    }
}