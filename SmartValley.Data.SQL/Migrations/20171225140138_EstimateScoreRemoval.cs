using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class EstimateScoreRemoval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estimates_Projects_ProjectId",
                table: "Estimates");

            migrationBuilder.DropForeignKey(
                name: "FK_Estimates_Questions_QuestionId",
                table: "Estimates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estimates",
                table: "Estimates");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Estimates");

            migrationBuilder.RenameTable(
                name: "Estimates",
                newName: "EstimateComments");

            migrationBuilder.RenameIndex(
                name: "IX_Estimates_QuestionId",
                table: "EstimateComments",
                newName: "IX_EstimateComments_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Estimates_ProjectId",
                table: "EstimateComments",
                newName: "IX_EstimateComments_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstimateComments",
                table: "EstimateComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_Projects_ProjectId",
                table: "EstimateComments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateComments_Questions_QuestionId",
                table: "EstimateComments",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_Projects_ProjectId",
                table: "EstimateComments");

            migrationBuilder.DropForeignKey(
                name: "FK_EstimateComments_Questions_QuestionId",
                table: "EstimateComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstimateComments",
                table: "EstimateComments");

            migrationBuilder.RenameTable(
                name: "EstimateComments",
                newName: "Estimates");

            migrationBuilder.RenameIndex(
                name: "IX_EstimateComments_QuestionId",
                table: "Estimates",
                newName: "IX_Estimates_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_EstimateComments_ProjectId",
                table: "Estimates",
                newName: "IX_Estimates_ProjectId");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Estimates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estimates",
                table: "Estimates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Estimates_Projects_ProjectId",
                table: "Estimates",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estimates_Questions_QuestionId",
                table: "Estimates",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
