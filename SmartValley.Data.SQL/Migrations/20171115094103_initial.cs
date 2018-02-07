using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CryptoCurrency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FinancialModelLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HardCap = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InvestmentsAreAttracted = table.Column<bool>(type: "bit", nullable: false),
                    MVPLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SoftCap = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    WhitePaperLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    FacebookLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LinkedInLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PersonType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorAddress = table.Column<string>(type: "nvarchar(42)", maxLength: 42, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProblemDesc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ProjectAddress = table.Column<string>(type: "nvarchar(42)", maxLength: 42, nullable: false),
                    ProjectArea = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SolutionDesc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
