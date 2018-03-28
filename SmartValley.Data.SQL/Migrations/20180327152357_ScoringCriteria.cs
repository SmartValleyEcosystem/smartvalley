using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ScoringCriteria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE EstimateComments;");
            migrationBuilder.Sql("TRUNCATE TABLE ScoringOffers;");
            migrationBuilder.Sql("TRUNCATE TABLE AreaScorings;");
            migrationBuilder.Sql("DELETE FROM Scorings;");

            migrationBuilder.Sql("UPDATE Areas SET MaxScore = 23 WHERE Id = 1; ");
            migrationBuilder.Sql("UPDATE Areas SET MaxScore = 27 WHERE Id = 2; ");
            migrationBuilder.Sql("UPDATE Areas SET MaxScore = 17 WHERE Id = 3; ");
            migrationBuilder.Sql("UPDATE Areas SET MaxScore = 16 WHERE Id = 4; ");
            
            migrationBuilder.Sql("INSERT INTO Areas (Id, Name, MaxScore) VALUES (5, 'Marketer', 17)");

            migrationBuilder.Sql("DELETE FROM ScoringCriteria;");
            
            migrationBuilder.DropColumn("QuestionIndex", "ScoringCriteria");

            migrationBuilder.Sql("SET IDENTITY_INSERT ScoringCriteria ON;");
            migrationBuilder.Sql(@"INSERT INTO ScoringCriteria (Id, AreaType, Weight)
                                    VALUES
                                    (1,  4, 10),
                                    (2,  4, 3),
                                    (3,  4, 1),
                                    (4,  4, 4),
                                    (5,  4, 4),
                                    (6,  4, 2),
                                    (7,  4, 8),
                                    (8,  4, 4),
                                    (9,  4, 8),
                                    (10, 4, 4),
                                    (11, 4, 7),
                                    (12, 4, 3),
                                    (13, 4, 3),

                                    (14, 1, 10),
                                    (15, 1, 10),
                                    (16, 1, 3),
                                    (17, 1, 4),
                                    (18, 1, 7),
                                    (19, 1, 3),
                                    (20, 1, 6),
                                    (21, 1, 5),
                                    (22, 1, 5),
                                    (23, 1, 2),
                                    (24, 1, 3),

                                    (25, 3, 5),
                                    (26, 3, 3),
                                    (27, 3, 10),
                                    (28, 3, 5),

                                    (29, 2, 10),
                                    (30, 2, 10),
                                    (31, 2, 7),
                                    (32, 2, 5),
                                    (33, 2, 6),
                                    (34, 2, 2),
                                    (35, 2, 5),
                                    (36, 2, 2),
                                    (37, 2, 2),
                                    (38, 2, 5),
                                    (39, 2, 3),
                                    (40, 2, 10),
                                    (41, 2, 10),
                                    (42, 2, 7),
                                    (43, 2, 5),
                                    (44, 2, 3),
                                    (45, 2, 8),
                                    (46, 2, 5),
                                    (47, 2, 3),
                                    (48, 2, 5),

                                    (49, 5, 3),
                                    (50, 5, 7),
                                    (51, 5, 9),
                                    (52, 5, 9),
                                    (53, 5, 10),
                                    (54, 5, 6),
                                    (55, 5, 10),
                                    (56, 5, 9),
                                    (57, 5, 6),
                                    (58, 5, 4),
                                    (59, 5, 4),
                                    (60, 5, 4),
                                    (61, 5, 4),
                                    (62, 5, 6),
                                    (63, 5, 4),
                                    (64, 5, 6),
                                    (65, 5, 8),
                                    (66, 5, 3),
                                    (67, 5, 5),
                                    (68, 5, 7)
                                    ");
            migrationBuilder.Sql("SET IDENTITY_INSERT ScoringCriteria OFF;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
