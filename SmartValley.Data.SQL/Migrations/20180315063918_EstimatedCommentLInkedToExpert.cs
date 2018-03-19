using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class EstimatedCommentLInkedToExpert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ExpertId",
                table: "EstimateComments",
                nullable: true);

            migrationBuilder.Sql(@" insert into Users (Address, Email, IsEmailConfirmed)
                                    select distinct ExpertAddress,ExpertAddress, 1
                                    from EstimateComments
                                    where ExpertAddress not in (select Address from users)
                                 ");

            migrationBuilder.Sql(@"insert into Experts (UserId,IsAvailable)
                                   select Id,1 from users
                                   where Id not in (select UserId from Experts)");

            migrationBuilder.Sql("update ec " +
                                 "set ec.ExpertId = u.Id " +
                                 "from dbo.EstimateComments ec " +
                                 "inner join dbo.Users u on ec.ExpertAddress = u.Address");

            migrationBuilder.AlterColumn<long>(
                name: "ExpertId",
                table: "EstimateComments",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_EstimateComments_ExpertId",
                table: "EstimateComments",
                column: "ExpertId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_Experts_ExpertId",
                table: "EstimateComments",
                column: "ExpertId",
                principalTable: "Experts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "ExpertAddress",
                table: "EstimateComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpertAddress",
                table: "EstimateComments",
                maxLength: 42,
                nullable: true);

            migrationBuilder.Sql("update ec " +
                                 "set ec.ExpertAddress = u.Address " +
                                 "from dbo.EstimateComments ec " +
                                 "inner join dbo.Users u on ec.ExpertId = u.Id");

            migrationBuilder.AlterColumn<string>(
                name: "ExpertAddress",
                table: "EstimateComments",
                maxLength: 42,
                nullable: false);

            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_Experts_ExpertId",
                table: "EstimateComments");

            migrationBuilder.DropIndex(
                name: "IX_EstimateComments_ExpertId",
                table: "EstimateComments");

            migrationBuilder.DropColumn(
                name: "ExpertId",
                table: "EstimateComments");
        }
    }
}