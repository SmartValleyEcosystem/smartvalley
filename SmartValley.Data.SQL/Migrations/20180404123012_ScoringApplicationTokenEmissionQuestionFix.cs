using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ScoringApplicationTokenEmissionQuestionFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE [dbo].[ScoringApplicationQuestions]
                                   SET [ParentTriggerValue] = N'0'
                                   WHERE [Key] = N'TokenEmissionRestrictionDescriptionQuestion'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE [dbo].[ScoringApplicationQuestions]
                                   SET [ParentTriggerValue] = N'1'
                                   WHERE [Key] = N'TokenEmissionRestrictionDescriptionQuestion'");
        }
    }
}
