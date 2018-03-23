using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddApplicationScoringQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoringApplicationQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(nullable: true),
                    ViewType = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    ParentTriggerValue = table.Column<string>(nullable: true),
                    GroupKey = table.Column<string>(nullable: true),
                    GroupOrder = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringApplicationQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoringApplications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Saved = table.Column<DateTime>(nullable: false),
                    Filled = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoringApplications_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoringApplicationAnswerses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ScoringApplicationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringApplicationAnswerses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoringApplicationAnswerses_ScoringApplicationQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "ScoringApplicationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoringApplicationAnswerses_ScoringApplications_ScoringApplicationId",
                        column: x => x.ScoringApplicationId,
                        principalTable: "ScoringApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplicationAnswerses_QuestionId",
                table: "ScoringApplicationAnswerses",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplicationAnswerses_ScoringApplicationId",
                table: "ScoringApplicationAnswerses",
                column: "ScoringApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplications_ProjectId",
                table: "ScoringApplications",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoringApplicationAnswerses");

            migrationBuilder.DropTable(
                name: "ScoringApplicationQuestions");

            migrationBuilder.DropTable(
                name: "ScoringApplications");
        }
    }
}
