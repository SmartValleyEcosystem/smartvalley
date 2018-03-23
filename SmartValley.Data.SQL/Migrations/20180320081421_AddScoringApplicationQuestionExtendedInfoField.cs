using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddScoringApplicationQuestionExtendedInfoField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoringApplicationAnswerses_ScoringApplicationQuestions_QuestionId",
                table: "ScoringApplicationAnswerses");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoringApplicationAnswerses_ScoringApplications_ScoringApplicationId",
                table: "ScoringApplicationAnswerses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScoringApplicationAnswerses",
                table: "ScoringApplicationAnswerses");

            migrationBuilder.RenameTable(
                name: "ScoringApplicationAnswerses",
                newName: "ScoringApplicationAnswers");

            migrationBuilder.RenameColumn(
                name: "ViewType",
                table: "ScoringApplicationQuestions",
                newName: "ExtendedInfo");

            migrationBuilder.RenameIndex(
                name: "IX_ScoringApplicationAnswerses_ScoringApplicationId",
                table: "ScoringApplicationAnswers",
                newName: "IX_ScoringApplicationAnswers_ScoringApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_ScoringApplicationAnswerses_QuestionId",
                table: "ScoringApplicationAnswers",
                newName: "IX_ScoringApplicationAnswers_QuestionId");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ScoringApplicationQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScoringApplicationAnswers",
                table: "ScoringApplicationAnswers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringApplicationAnswers_ScoringApplicationQuestions_QuestionId",
                table: "ScoringApplicationAnswers",
                column: "QuestionId",
                principalTable: "ScoringApplicationQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringApplicationAnswers_ScoringApplications_ScoringApplicationId",
                table: "ScoringApplicationAnswers",
                column: "ScoringApplicationId",
                principalTable: "ScoringApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoringApplicationAnswers_ScoringApplicationQuestions_QuestionId",
                table: "ScoringApplicationAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoringApplicationAnswers_ScoringApplications_ScoringApplicationId",
                table: "ScoringApplicationAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScoringApplicationAnswers",
                table: "ScoringApplicationAnswers");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ScoringApplicationQuestions");

            migrationBuilder.RenameTable(
                name: "ScoringApplicationAnswers",
                newName: "ScoringApplicationAnswerses");

            migrationBuilder.RenameColumn(
                name: "ExtendedInfo",
                table: "ScoringApplicationQuestions",
                newName: "ViewType");

            migrationBuilder.RenameIndex(
                name: "IX_ScoringApplicationAnswers_ScoringApplicationId",
                table: "ScoringApplicationAnswerses",
                newName: "IX_ScoringApplicationAnswerses_ScoringApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_ScoringApplicationAnswers_QuestionId",
                table: "ScoringApplicationAnswerses",
                newName: "IX_ScoringApplicationAnswerses_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScoringApplicationAnswerses",
                table: "ScoringApplicationAnswerses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringApplicationAnswerses_ScoringApplicationQuestions_QuestionId",
                table: "ScoringApplicationAnswerses",
                column: "QuestionId",
                principalTable: "ScoringApplicationQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringApplicationAnswerses_ScoringApplications_ScoringApplicationId",
                table: "ScoringApplicationAnswerses",
                column: "ScoringApplicationId",
                principalTable: "ScoringApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
