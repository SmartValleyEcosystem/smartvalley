using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class FixingScoringDeadlineField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update Scorings
                                   set ScoringDeadline = DATEADD(d, 4, CreationDate)");

            migrationBuilder.Sql(@"alter table Scorings
                                   alter column ScoringDeadline datetimeoffset(7) not null");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
