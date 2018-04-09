using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ScoringCriteriaGrouping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM EstimateComments");

            migrationBuilder.Sql(@"DELETE FROM ScoringCriteria");

            migrationBuilder.AddColumn<string>(
                name: "GroupKey",
                table: "ScoringCriteria");

            migrationBuilder.AddColumn<int>(
                name: "GroupOrder",
                table: "ScoringCriteria");

            migrationBuilder.AddColumn<bool>(
                name: "HasMiddleScoreOption",
                table: "ScoringCriteria",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ScoringCriteria");

            migrationBuilder.Sql("SET IDENTITY_INSERT ScoringCriteria ON;");

            migrationBuilder.Sql(@"INSERT INTO [dbo].[ScoringCriteria] ([Id], [AreaType], [Weight], [GroupKey], [GroupOrder], [Order], [HasMiddleScoreOption])
                                    VALUES
                                    (1 , 4, 10, N'Token', 1              , 1 , 1),
                                    (2 , 4, 3 , N'Token', 1              , 2 , 1),
                                    (3 , 4, 1 , N'CompanyAndTokenSale', 2, 3 , 1),
                                    (4 , 4, 4 , N'CompanyAndTokenSale', 2, 4 , 1),
                                    (5 , 4, 4 , N'CompanyAndTokenSale', 2, 5 , 1),
                                    (6 , 4, 2 , N'CompanyAndTokenSale', 2, 6 , 1),
                                    (7 , 4, 8 , N'Documents', 3          , 7 , 1),
                                    (8 , 4, 4 , N'Documents', 3          , 8 , 1),
                                    (9 , 4, 8 , N'Documents', 3          , 9 , 1),
                                    (10, 4, 4 , N'Documents', 3          , 10, 1),
                                    (11, 4, 7 , N'Marketing', 4          , 11, 0),
                                    (12, 4, 3 , N'Marketing', 4          , 12, 0),
                                    (13, 4, 3 , N'Marketing', 4          , 13, 0)
                                    ");

            migrationBuilder.Sql(@"INSERT INTO [dbo].[ScoringCriteria] ([Id], [AreaType], [Weight], [GroupKey], [GroupOrder], [Order], [HasMiddleScoreOption])
                                    VALUES
                                    (14, 1, 10, N'Team', 1               , 1 , 0),
                                    (15, 1, 10, N'AdvisoryBoard', 2      , 2 , 1),
                                    (16, 1, 3 , N'AdvisoryBoard', 2      , 3 , 0),
                                    (17, 1, 4 , N'AdvisoryBoard', 2      , 4 , 1),
                                    (18, 1, 7 , N'Team', 3               , 5 , 1),
                                    (19, 1, 3 , N'Team', 3               , 6 , 1),
                                    (20, 1, 6 , N'Team', 3               , 7 , 0),
                                    (21, 1, 5 , N'Team', 3               , 8 , 1),
                                    (22, 1, 5 , N'Team', 3               , 9 , 1),
                                    (23, 1, 2 , N'Team', 3               , 10, 0),
                                    (24, 1, 3 , N'Team', 3               , 11, 1)
                                    ");

            migrationBuilder.Sql(@"INSERT INTO [dbo].[ScoringCriteria] ([Id], [AreaType], [Weight], [GroupKey], [GroupOrder], [Order], [HasMiddleScoreOption])
                                    VALUES
                                    (25, 3, 5 , N'Tech', 1               , 1, 1),
                                    (26, 3, 3 , N'Tech', 1               , 2, 1),
                                    (27, 3, 10, N'Tech', 1               , 3, 1),
                                    (28, 3, 5 , N'Team', 2               , 4, 1)
                                    ");

            migrationBuilder.Sql(@"INSERT INTO [dbo].[ScoringCriteria] ([Id], [AreaType], [Weight], [GroupKey], [GroupOrder], [Order], [HasMiddleScoreOption])
                                    VALUES
                                    (29, 2, 10, N'Product', 1            , 1 , 1),
                                    (30, 2, 10, N'Product', 1            , 2 , 1),
                                    (31, 2, 7 , N'Product', 1            , 3 , 1),
                                    (32, 2, 5 , N'Product', 1            , 4 , 1),
                                    (33, 2, 6 , N'Product', 1            , 5 , 1),
                                    (34, 2, 2 , N'Product', 1            , 6 , 1),
                                    (35, 2, 5 , N'Product', 1            , 7 , 1),
                                    (36, 2, 2 , N'Team', 2                , 8 , 1),
                                    (37, 2, 2 , N'Team', 2                , 9 , 0),
                                    (38, 2, 5 , N'Token', 3              , 10, 1),
                                    (39, 2, 3 , N'Token', 3              , 11, 1),
                                    (40, 2, 10, N'Token', 3              , 12, 0),
                                    (41, 2, 10, N'Token', 3              , 13, 0),
                                    (42, 2, 7 , N'Token', 3              , 14, 0),
                                    (43, 2, 5 , N'Token', 3              , 15, 0),
                                    (44, 2, 3 , N'Ico', 4                , 16, 1),
                                    (45, 2, 8 , N'Ico', 4                , 17, 1),
                                    (46, 2, 5 , N'Ico', 4                , 18, 1),
                                    (47, 2, 3 , N'Ico', 4                , 19, 1),
                                    (48, 2, 5 , N'Ico', 4                , 20, 1)
                                    ");

            migrationBuilder.Sql(@"INSERT INTO [dbo].[ScoringCriteria] ([Id], [AreaType], [Weight], [GroupKey], [GroupOrder], [Order], [HasMiddleScoreOption])
                                    VALUES
                                    (49, 5, 3 , N'Project', 1            , 1 , 1),
                                    (50, 5, 7 , N'Project', 1            , 2 , 1),
                                    (51, 5, 9 , N'Project', 1            , 3 , 1),
                                    (52, 5, 9 , N'Project', 1            , 4 , 1),
                                    (53, 5, 10, N'Project', 1            , 5 , 1),
                                    (54, 5, 6 , N'Product', 2            , 6 , 1),
                                    (55, 5, 10, N'Product', 2            , 7 , 1),
                                    (56, 5, 9 , N'Product', 2            , 8 , 1),
                                    (57, 5, 6 , N'Product', 2            , 9 , 1),
                                    (58, 5, 4 , N'Product', 2            , 10, 1),
                                    (59, 5, 4 , N'Product', 2            , 11, 1),
                                    (60, 5, 4 , N'Product', 2            , 12, 1),
                                    (61, 5, 4 , N'Product', 2            , 13, 1),
                                    (62, 5, 6 , N'Product', 2            , 14, 1),
                                    (63, 5, 4 , N'Product', 2            , 15, 1),
                                    (64, 5, 6 , N'Product', 2            , 16, 1),
                                    (65, 5, 8 , N'Product', 2            , 17, 1),
                                    (66, 5, 3 , N'Product', 2            , 18, 1),
                                    (67, 5, 5 , N'Ico', 3                , 19, 1),
                                    (68, 5, 7 , N'Token', 4              , 20, 1)
                                    ");

            migrationBuilder.Sql("SET IDENTITY_INSERT ScoringCriteria OFF;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupKey",
                table: "ScoringCriteria");

            migrationBuilder.DropColumn(
                name: "GroupOrder",
                table: "ScoringCriteria");

            migrationBuilder.DropColumn(
                name: "HasMiddleScoreOption",
                table: "ScoringCriteria");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "ScoringCriteria");
        }
    }
}