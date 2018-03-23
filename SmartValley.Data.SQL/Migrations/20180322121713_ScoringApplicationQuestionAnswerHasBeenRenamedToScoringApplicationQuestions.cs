using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ScoringApplicationQuestionAnswerHasBeenRenamedToScoringApplicationQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoringApplicationAnswer");

            migrationBuilder.CreateTable(
                name: "ScoringApplicationAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ScoringApplicationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringApplicationAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoringApplicationAnswers_ScoringApplicationQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "ScoringApplicationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoringApplicationAnswers_ScoringApplications_ScoringApplicationId",
                        column: x => x.ScoringApplicationId,
                        principalTable: "ScoringApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplicationAnswers_QuestionId",
                table: "ScoringApplicationAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplicationAnswers_ScoringApplicationId",
                table: "ScoringApplicationAnswers",
                column: "ScoringApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoringApplicationAnswers");

            migrationBuilder.CreateTable(
                name: "ScoringApplicationAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<long>(nullable: false),
                    ScoringApplicationId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringApplicationAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoringApplicationAnswer_ScoringApplicationQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "ScoringApplicationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoringApplicationAnswer_ScoringApplications_ScoringApplicationId",
                        column: x => x.ScoringApplicationId,
                        principalTable: "ScoringApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplicationAnswer_QuestionId",
                table: "ScoringApplicationAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplicationAnswer_ScoringApplicationId",
                table: "ScoringApplicationAnswer",
                column: "ScoringApplicationId");
        }
    }
}
