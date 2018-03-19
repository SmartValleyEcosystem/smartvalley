using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddCountriesStagesSocials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectArea",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Projects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "CountryId",
                table: "Projects",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "StageId",
                table: "Projects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ApplicationTeamMembers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationId = table.Column<long>(nullable: false),
                    FullName = table.Column<string>(maxLength: 100, nullable: false),
                    Role = table.Column<string>(maxLength: 100, nullable: false),
                    About = table.Column<string>(maxLength: 500, nullable: true),
                    PhotoName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationTeamMembers_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.Sql(@"INSERT INTO Categories (Name, Id) values
                                ('Art',0),
                                ('ArtificialIntelligence',1),
                                ('Banking',2),
                                ('BigData',3),
                                ('BusinessServices',4),
                                ('CasinoAndGambling',5),
                                ('Charity',6),
                                ('Communication',7),
                                ('Cryptocurrency',8),
                                ('Education',9),
                                ('Electronics',10),
                                ('Energy',11),
                                ('Entertainment',12),
                                ('Health',13),
                                ('Infrastructure',14),
                                ('Internet',15),
                                ('Investment',16),
                                ('Legal',17),
                                ('Manufacturing',18),
                                ('Media',19),
                                ('Other',20),
                                ('Platform',21),
                                ('RealEstate',22),
                                ('Retail',23),
                                ('SmartContract',24),
                                ('Software',25),
                                ('Sports',26),
                                ('Tourism',27),
                                ('VirtualReality',28)
                        ");

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.Sql(@"INSERT INTO Countries (Code) values
                                ('AF'),
                                ('AX'),
                                ('AL'),
                                ('DZ'),
                                ('AS'),
                                ('AD'),
                                ('AO'),
                                ('AI'),
                                ('AQ'),
                                ('AG'),
                                ('AR'),
                                ('AM'),
                                ('AW'),
                                ('AU'),
                                ('AT'),
                                ('AZ'),
                                ('BS'),
                                ('BH'),
                                ('BD'),
                                ('BB'),
                                ('BY'),
                                ('BE'),
                                ('BZ'),
                                ('BJ'),
                                ('BM'),
                                ('BT'),
                                ('BO'),
                                ('BQ'),
                                ('BA'),
                                ('BW'),
                                ('BV'),
                                ('BR'),
                                ('IO'),
                                ('BN'),
                                ('BG'),
                                ('BF'),
                                ('BI'),
                                ('KH'),
                                ('CM'),
                                ('CA'),
                                ('CV'),
                                ('KY'),
                                ('CF'),
                                ('TD'),
                                ('CL'),
                                ('CN'),
                                ('CX'),
                                ('CC'),
                                ('CO'),
                                ('KM'),
                                ('CG'),
                                ('CD'),
                                ('CK'),
                                ('CR'),
                                ('CI'),
                                ('HR'),
                                ('CU'),
                                ('CW'),
                                ('CY'),
                                ('CZ'),
                                ('DK'),
                                ('DJ'),
                                ('DM'),
                                ('DO'),
                                ('EC'),
                                ('EG'),
                                ('SV'),
                                ('GQ'),
                                ('ER'),
                                ('EE'),
                                ('ET'),
                                ('FK'),
                                ('FO'),
                                ('FJ'),
                                ('FI'),
                                ('FR'),
                                ('GF'),
                                ('PF'),
                                ('TF'),
                                ('GA'),
                                ('GM'),
                                ('GE'),
                                ('DE'),
                                ('GH'),
                                ('GI'),
                                ('GR'),
                                ('GL'),
                                ('GD'),
                                ('GP'),
                                ('GU'),
                                ('GT'),
                                ('GG'),
                                ('GN'),
                                ('GW'),
                                ('GY'),
                                ('HT'),
                                ('HM'),
                                ('VA'),
                                ('HN'),
                                ('HK'),
                                ('HU'),
                                ('IS'),
                                ('IN'),
                                ('ID'),
                                ('IR'),
                                ('IQ'),
                                ('IE'),
                                ('IM'),
                                ('IL'),
                                ('IT'),
                                ('JM'),
                                ('JP'),
                                ('JE'),
                                ('JO'),
                                ('KZ'),
                                ('KE'),
                                ('KI'),
                                ('KP'),
                                ('KR'),
                                ('KW'),
                                ('KG'),
                                ('LA'),
                                ('LV'),
                                ('LB'),
                                ('LS'),
                                ('LR'),
                                ('LY'),
                                ('LI'),
                                ('LT'),
                                ('LU'),
                                ('MO'),
                                ('MK'),
                                ('MG'),
                                ('MW'),
                                ('MY'),
                                ('MV'),
                                ('ML'),
                                ('MT'),
                                ('MH'),
                                ('MQ'),
                                ('MR'),
                                ('MU'),
                                ('YT'),
                                ('MX'),
                                ('FM'),
                                ('MD'),
                                ('MC'),
                                ('MN'),
                                ('ME'),
                                ('MS'),
                                ('MA'),
                                ('MZ'),
                                ('MM'),
                                ('NA'),
                                ('NR'),
                                ('NP'),
                                ('NL'),
                                ('NC'),
                                ('NZ'),
                                ('NI'),
                                ('NE'),
                                ('NG'),
                                ('NU'),
                                ('NF'),
                                ('MP'),
                                ('NO'),
                                ('OM'),
                                ('PK'),
                                ('PW'),
                                ('PS'),
                                ('PA'),
                                ('PG'),
                                ('PY'),
                                ('PE'),
                                ('PH'),
                                ('PN'),
                                ('PL'),
                                ('PT'),
                                ('PR'),
                                ('QA'),
                                ('RE'),
                                ('RO'),
                                ('RU'),
                                ('RW'),
                                ('BL'),
                                ('SH'),
                                ('KN'),
                                ('LC'),
                                ('MF'),
                                ('PM'),
                                ('VC'),
                                ('WS'),
                                ('SM'),
                                ('ST'),
                                ('SA'),
                                ('SN'),
                                ('RS'),
                                ('SC'),
                                ('SL'),
                                ('SG'),
                                ('SX'),
                                ('SK'),
                                ('SI'),
                                ('SB'),
                                ('SO'),
                                ('ZA'),
                                ('GS'),
                                ('SS'),
                                ('ES'),
                                ('LK'),
                                ('SD'),
                                ('SR'),
                                ('SJ'),
                                ('SZ'),
                                ('SE'),
                                ('CH'),
                                ('SY'),
                                ('TW'),
                                ('TJ'),
                                ('TZ'),
                                ('TH'),
                                ('TL'),
                                ('TG'),
                                ('TK'),
                                ('TO'),
                                ('TT'),
                                ('TN'),
                                ('TR'),
                                ('TM'),
                                ('TC'),
                                ('TV'),
                                ('UG'),
                                ('UA'),
                                ('AE'),
                                ('GB'),
                                ('US'),
                                ('UM'),
                                ('UY'),
                                ('UZ'),
                                ('VU'),
                                ('VE'),
                                ('VN'),
                                ('VG'),
                                ('VI'),
                                ('WF'),
                                ('EH'),
                                ('YE'),
                                ('ZM'),
                                ('ZW')");

            migrationBuilder.CreateTable(
                name: "ProjectTeamMembers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<long>(nullable: false),
                    FullName = table.Column<string>(maxLength: 100, nullable: false),
                    Role = table.Column<string>(maxLength: 100, nullable: false),
                    About = table.Column<string>(maxLength: 500, nullable: true),
                    PhotoName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTeamMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialMedias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                });
            
            migrationBuilder.Sql(@"INSERT INTO Stages (Name, Id) values
                                ('PreSale',1)
                        ");

            migrationBuilder.CreateTable(
                name: "ApplicationTeamMemberSocialMedias",
                columns: table => new
                {
                    TeamMemberId = table.Column<long>(nullable: false),
                    SocialMediaId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTeamMemberSocialMedias", x => new { x.SocialMediaId, x.TeamMemberId });
                    table.ForeignKey(
                        name: "FK_ApplicationTeamMemberSocialMedias_SocialMedias_SocialMediaId",
                        column: x => x.SocialMediaId,
                        principalTable: "SocialMedias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationTeamMemberSocialMedias_ProjectTeamMembers_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "ProjectTeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSocialMedias",
                columns: table => new
                {
                    ProjectId = table.Column<long>(nullable: false),
                    SocialId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSocialMedias", x => new { x.SocialId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectSocialMedias_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectSocialMedias_SocialMedias_SocialId",
                        column: x => x.SocialId,
                        principalTable: "SocialMedias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTeamMemberSocialMedias",
                columns: table => new
                {
                    TeamMemberId = table.Column<long>(nullable: false),
                    SocialMediaId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeamMemberSocialMedias", x => new { x.SocialMediaId, x.TeamMemberId });
                    table.ForeignKey(
                        name: "FK_ProjectTeamMemberSocialMedias_SocialMedias_SocialMediaId",
                        column: x => x.SocialMediaId,
                        principalTable: "SocialMedias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTeamMemberSocialMedias_ProjectTeamMembers_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "ProjectTeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CountryId",
                table: "Projects",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StageId",
                table: "Projects",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTeamMembers_ApplicationId",
                table: "ApplicationTeamMembers",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTeamMemberSocialMedias_TeamMemberId",
                table: "ApplicationTeamMemberSocialMedias",
                column: "TeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTeamMemberSocialMedias_SocialMediaId_TeamMemberId",
                table: "ApplicationTeamMemberSocialMedias",
                columns: new[] { "SocialMediaId", "TeamMemberId" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSocialMedias_ProjectId",
                table: "ProjectSocialMedias",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSocialMedias_SocialId_ProjectId",
                table: "ProjectSocialMedias",
                columns: new[] { "SocialId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamMembers_ProjectId",
                table: "ProjectTeamMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamMemberSocialMedias_TeamMemberId",
                table: "ProjectTeamMemberSocialMedias",
                column: "TeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamMemberSocialMedias_SocialMediaId_TeamMemberId",
                table: "ProjectTeamMemberSocialMedias",
                columns: new[] { "SocialMediaId", "TeamMemberId" });

            migrationBuilder.CreateIndex(
                name: "IX_SocialMedias_Name",
                table: "SocialMedias",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stages_Name",
                table: "Stages",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.Sql(@"update Projects set CountryId = 1");
            migrationBuilder.Sql(@"update Projects set StageId = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Categories_CategoryId",
                table: "Projects",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Countries_CountryId",
                table: "Projects",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Stages_StageId",
                table: "Projects",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Categories_CategoryId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Countries_CountryId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Stages_StageId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "ApplicationTeamMembers");

            migrationBuilder.DropTable(
                name: "ApplicationTeamMemberSocialMedias");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "ProjectSocialMedias");

            migrationBuilder.DropTable(
                name: "ProjectTeamMemberSocialMedias");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "SocialMedias");

            migrationBuilder.DropTable(
                name: "ProjectTeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CountryId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_StageId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Projects",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectArea",
                table: "Projects",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationId = table.Column<long>(nullable: false),
                    FacebookLink = table.Column<string>(maxLength: 200, nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: false),
                    LinkedInLink = table.Column<string>(maxLength: 200, nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_ApplicationId",
                table: "TeamMembers",
                column: "ApplicationId");
        }
    }
}
