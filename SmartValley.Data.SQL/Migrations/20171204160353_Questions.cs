using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class Questions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                                  {
                                      Id = table.Column<long>(type: "bigint", nullable: false)
                                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                                      Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                                      QuestionIndex = table.Column<int>(type: "int", nullable: false),
                                      ExpertiseArea = table.Column<int>(type: "int", nullable: false),
                                      MaxScore = table.Column<int>(type: "int", nullable: false),
                                      MinScore = table.Column<int>(type: "int", nullable: false),
                                      Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                                  },
                constraints: table => { table.PrimaryKey("PK_Questions", x => x.Id); });

            migrationBuilder.AddColumn<long>(
                name: "QuestionId",
                table: "Estimates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.Sql(@"INSERT INTO Questions (Name, Description, ExpertiseArea, MinScore, MaxScore, QuestionIndex)
                                    VALUES
                                    ('Team completeness','In case all major roles are filled in - <strong>6</strong> points<br/>In case any position is missing, minus <strong>1</strong> point for each specialist, minus <strong>2</strong> points for CEO<br/>In case specialist doesn''t have any experience, disregard him<br/> <br/>Minimum: <strong>0</strong> points, Maximum: <strong>6</strong> points', 1, 0, 6,0),
                                    ('Team experience', 'Team experience (for each position)<br/> Specialist''s experience:<br/> <span class=""sub-list"">Less than 2 years - <strong>1</strong> point</span><br/> <span class=""sub-list"">More than 2 years - <strong>2</strong> points</span><br/> <span class=""sub-list"">No experience - <strong>0</strong> points</span><br/> <br/>Minimum: <strong>0</strong> points, Maximum: <strong>10</strong> points<br/>', 1, 0, 10,1),
                                    ('Attracted investments','In case anyone in the team attracted investments before - <strong>3</strong> points<br/><br/>Minimum: <strong>0</strong> points, Maximum: <strong>3</strong> points', 1, 0, 3,2),
                                    ('Scam','In case anyone in the team was noticed in scam projects - minus <strong>15</strong> points<br/>In case no one in the team was noticed in scam projects - <strong>0</strong> points<br/><br/>Minumum: <strong>-15</strong> points, Maximum: <strong>0</strong> points', 1, -15, 0, 3),

                                    ('Incorporation risk','In case incorporation has minimal risk - <strong>10</strong> points<br/>In case incorporation has medium risk - <strong>6</strong> points<br/>In case incorporation has high risk - <strong>2</strong> points<br/>Project has no incorporation - <strong>0</strong> points<br/><br/>Minimum: <strong>0</strong> points, Maximum: <strong>10</strong> points', 4, 0, 10,0),
                                    ('Token structure','In case of Utility token - <strong>15</strong> points<br/>In case of Security token, but company has SEC license - <strong>5</strong> points<br/>In case of Security token - <strong>0</strong> points<br/><br/>Minimum: <strong>0</strong> points, Maximum: <strong>15</strong> points', 4, 0, 15,1),

                                    ('Amount involved','In case amount involved is commensurate with the project''s goals and is less than 15 mln. USD - <strong>8</strong> points<br/>In case amount involved is commensurate with the project''s goals and is more than 15 mln. USD - <strong>6</strong> points<br/>In case amount involved is disproportionate with the project''s goals and is less than 15 mln. USD - <strong>4</strong> points<br/>In case amount involved is disproportionate with the project''s goals and more than 15 mln. USD - <strong>2</strong> points<br/><br/>Minimum: <strong>2</strong> points, Maximum: <strong>8</strong> points', 2, 0, 8,0),
                                    ('Financial model analysis','Efficient financial model with exponential scaled income growth - <strong>10</strong> points<br/>Efficient financial model with linear scaled income growth - <strong>6</strong> points<br/>No efficient finance model - <strong>0</strong> points<br/><br/>Minimum: <strong>0</strong> points, Maximum: <strong>10</strong> points', 2, 0, 10,1),
                                    ('Idea and it''s implementation analysis in terms of business','Project has economic advantages and doesn''t have competitors - <strong>10</strong> points<br/>Project has economic advantages - <strong>7</strong> points<br/>Project has no economic advantages, but has other advantages - <strong>4</strong> points<br/>Project has no advantages - <strong>0</strong> points<br/><br/>Minimum: <strong>0</strong> points, Maximum: <strong>10</strong> points', 2, 0, 10,2),

                                    ('Blockchain use effectiveness', 'Blockchain is required - <strong>5</strong> points<br/>Blockchain solves minor problems - <strong>2</strong> points<br/>Blockchain is not required - <strong>0</strong> points<br/><br/>Minimum: <strong>0</strong> points, Maximum: <strong>5</strong> points', 3, 0, 5,0),
                                    ('Implementation on existing blockchains', 'Can be realised on existing blockchain protocols - <strong>6</strong> points<br/>Can''t be realised on existing blockchain protocols - <strong>0</strong> points<br/><br/>Minimum: <strong>0</strong> points, Maximum: <strong>6</strong> points', 3, 0, 6,1),
                                    ('Blockchain selection', 'Correct blockchain selected, protocol provides a way to realize all functionality - <strong>5</strong> points<br/>Correct blockchain selected, protocol provides a way to realize only main functionality - <strong>3</strong> points<br/>Uncorrect blockchain selected - <strong>0</strong> points<br/><br/>Minimum: <strong>0</strong> points, Maximum <strong>5</strong> points', 3, 0, 5,2),
                                    ('Prototype/MVP analysis', 'Prototype solves stated problem by means of blockchain - <strong>10</strong> points<br/>Prototype solves stated problem without blockchain use - <strong>6</strong> points<br/>Prototype does not solve stated problem, however blockchain is used - <strong>2</strong> points<br/>Prototype does not solve stated problem - <strong>0</strong> points<br/><br/>Minimum: <strong>0</strong> points, Maximum <strong>10</strong> points', 3, 0, 10,3)
                                    ");

            migrationBuilder.Sql(@"
                                    DECLARE @cursor CURSOR;
                                    DECLARE @category int;
                                    DECLARE @index int;                                   
                                    BEGIN
	                                    SET @cursor = CURSOR FOR
	                                    SELECT
		                                    QuestionIndex,
		                                    ScoringCategory
	                                    FROM Estimates

	                                    OPEN @cursor
	                                    FETCH NEXT FROM @cursor
	                                    INTO @index, @category

	                                    WHILE @@FETCH_STATUS = 0
	                                    BEGIN

		                                    UPDATE Estimates
		                                    SET QuestionId = (SELECT
			                                    Id
		                                    FROM Questions AS q
		                                    WHERE q.ExpertiseArea = @category
		                                    AND q.QuestionIndex = @index)
		                                    WHERE QuestionIndex = @index
		                                    AND ScoringCategory = @category

		                                    FETCH NEXT FROM @cursor
		                                    INTO @index, @category
	                                    END;

	                                    CLOSE @cursor;
	                                    DEALLOCATE @cursor;
                                    END;");

            migrationBuilder.DropColumn(
                name: "QuestionIndex",
                table: "Estimates");

            migrationBuilder.DropColumn(
                name: "ScoringCategory",
                table: "Estimates");

            migrationBuilder.CreateIndex(
                name: "IX_Estimates_QuestionId",
                table: "Estimates",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estimates_Questions_QuestionId",
                table: "Estimates",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estimates_Questions_QuestionId",
                table: "Estimates");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Estimates_QuestionId",
                table: "Estimates");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Estimates");

            migrationBuilder.AddColumn<int>(
                name: "QuestionIndex",
                table: "Estimates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpertiseArea",
                table: "Estimates");
        }
    }
}