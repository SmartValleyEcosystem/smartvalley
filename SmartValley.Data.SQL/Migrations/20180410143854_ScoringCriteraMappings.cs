using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ScoringCriteraMappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoringCriteriaMappings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScoringCriterionId = table.Column<long>(nullable: false),
                    ScoringApplicationQuestionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringCriteriaMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoringCriteriaMappings_ScoringApplicationQuestions_ScoringApplicationQuestionId",
                        column: x => x.ScoringApplicationQuestionId,
                        principalTable: "ScoringApplicationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoringCriteriaMappings_ScoringCriteria_ScoringCriterionId",
                        column: x => x.ScoringCriterionId,
                        principalTable: "ScoringCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoringCriteriaMappings_ScoringApplicationQuestionId",
                table: "ScoringCriteriaMappings",
                column: "ScoringApplicationQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringCriteriaMappings_ScoringCriterionId",
                table: "ScoringCriteriaMappings",
                column: "ScoringCriterionId");

            migrationBuilder.Sql(@"INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 1,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in (
                                'TokenFunctionsDescriptionQuestion',
                                'TokenTypeQuestion',
                                'LicensingForSecurityTokenQuestion',
                                'BuyerTokenValueQuestion',
                                'DoYouHaveTokenEmissionRestrictionQuestion',
                                'TokenEmissionRestrictionDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 2,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('WhenUserCanUseTokensQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 3,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('CountriesNotForFundingQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 4,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('WillYouArrangeKYCQuestion','KYCDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 5,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('CountryOfIncorporationQuestion','LinkToCompanyRegistrationDocumentQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 6,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouNeedAdditionalLicencesQuestion','AdditionalLicencesDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 7,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveTokenSaleAgreementQuestion','TokenSaleAgreementLinkQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 8,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveLegalOpinionQuestion','LegalOpinionLinkQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 9,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveAgreementOfRiskAssessmentQuestion','AgreementOfRiskAssessmentLinkQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 10,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveTermsAndConditionsQuestion','TermsAndConditionsLinkQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 11,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('HaveYouEverMadeProfitPromisesQuestion','ProfitPromisesDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 12,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('MarketingActivitiesCountriesQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 13,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('HaveYouEverMadeStockExchangePromisesQuestion','StockExchangePromisesDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 19,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoesYourTeamMembersHaveOtherIcoProjectExperienceQuestion','TeamMembersOtherIcoProjectExperienceDesciprionQuestion')


                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 20,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoesYourTeamMembersHaveOtherIcoProjectExperienceQuestion','TeamMembersOtherIcoProjectExperienceDesciprionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 21,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoesYourTeamHasExperienceInProjectAreaQuestion','TeamProjectAreaExperienceDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 22,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouNeedAdditionalEmployeesQuestion','AdditionalEmployeesWorkplaceDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 23,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoesYourTeamMembersHaveFundingAttractingExperienceQuestion','TeamMembersFundingAttractingExperienceDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 24,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouUseOutsourcingQuestion','DescribeOutsourcingPurposeQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 25,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('UsedBlockchainTypeQuestion','PlanningCountUsersToAttractQuestion','PlanningPayloadQuestion','PlanForPayloadIncreasingQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 26,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveMVPQuestion','MVPFeatureDescriptionQuestion','LinkToMVPQuestion','DoYouHaveMVPSourceCodesQuestion','MVPSourceCodesLinkQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 27,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveUniqueTechnologyQuestion','UniqueTechnologyDescriptionQuestion','DoYouhaveICORoadmapQuestion','ICORoadmapDescriptionLinkQuestion','DoYouHaveAlgorithmsSourceCodeQuestion','AlgorithmsSourceCodeLinkQuestion','DoYouHaveFuturePlatformTechnicalDescriptionQuestion','FuturePlatformTechnicalDescriptionLinkQuestion','TokenFunctionsDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 28,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouUseOutsourcingQuestion','DescribeOutsourcingPurposeQuestion','DoesYourTeamMembersHaveFundingAttractingExperienceQuestion','TeamMembersFundingAttractingExperienceDescriptionQuestion','DoesYourTeamMembersHaveFinancialOffencesQuestion','TeamMembersFinancialOffencesDescriptionQuestion','DoYouHaveAlgorithmsSourceCodeQuestion','AlgorithmsSourceCodeLinkQuestion','DoYouHaveFuturePlatformTechnicalDescriptionQuestion','FuturePlatformTechnicalDescriptionLinkQuestion','PlanningCountUsersToAttractQuestion','PlanningPayloadQuestion','PlanForPayloadIncreasingQuestion'',DoYouHaveMVPQuestion','MVPFeatureDescriptionQuestion','LinkToMVPQuestion','DoYouHaveMVPSourceCodesQuestion','MVPSourceCodesLinkQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 29,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoesYourProductSolveConcreteProblemQuestion','CanYouProveThatProblemExistsQuestion','LinksToProblemDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 30,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('WaysToSolveProblemWithoutBlockchainQuestion','ThreeWaysToUseThisProductQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 31,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveBusinessModelQuestion','BusinessModelDescriptionQuestion','DoYouHaveMarketingPlanQuestion','LinkToMarketingPlanQuestion','DoYouHaveFinancialModelQuestion','FinancialModelLinkQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 32,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveFinancialModelQuestion','FinancialModelLinkQuestion','LinksToFinancialModelResearchQuestion','DoYouHaveMarketingPlanQuestion','LinkToMarketingPlanQuestion'',TargetMarketSizeQuestion','TargetAudienceQuestion','MarketGeographyQuestion','DoYouHaveRealUsersQuestion','HowMuchRealUsersDoYouHaveQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 33,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('CurrentlyAvailableResources','DoYouHaveFinancialModelQuestion','FinancialModelLinkQuestion','LinksToFinancialModelResearchQuestion','DoYouHaveMVPQuestion','MVPFeatureDescriptionQuestion','LinkToMVPQuestion','MVPSpendedMoneyQuestion','MVPPlanningTimeSpendedQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 34,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveUniqueTechnologyQuestion','UniqueTechnologyDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 35,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouhaveICORoadmapQuestion','ICORoadmapDescriptionLinkQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 36,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouUseOutsourcingQuestion','DescribeOutsourcingPurposeQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 37,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveMVPQuestion','MVPFeatureDescriptionQuestion','LinkToMVPQuestion','MVPSpendedMoneyQuestion','MVPPlanningTimeSpendedQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 38,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('TokenFunctionsDescriptionQuestion','BuyerTokenValueQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 39,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('TokenValueGrowthFactorsQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 40,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('TokenNecessityForProjectQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 41,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('TokenChangingToETHQuestion')


                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 42,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveTokenEmissionRestrictionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 43,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('TokenPriceQuestion','TokenQuantityLimitationQuestion','CrowdsaleDescriptionQuestion','TokenDistributionPlanQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 44,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('CollectingFundsPlanQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 45,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('MinimumIcoRaiseAmountQuestion','MaximumIcoRaiseAmountQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 46,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('IcoRaisedFundsSpendPurposeQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 47,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('TokenDistributionPlanQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 48,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('CrowdsaleDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 50,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveMarketingPlanQuestion','LinkToMarketingPlanQuestion')


                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 51,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('TargetMarketSizeQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 52,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('TargetAudienceQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 53,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveBusinessModelQuestion','BusinessModelDescriptionQuestion','DoYouHaveMarketingPlanQuestion','LinkToMarketingPlanQuestion','DoYouHaveFinancialModelQuestion','FinancialModelLinkQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 54,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouhaveICORoadmapQuestion','ICORoadmapDescriptionLinkQuestion','DoYouHaveFinancialModelQuestion','FinancialModelLinkQuestion','DoYouHaveBusinessModelQuestion','BusinessModelDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 55,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoesYourProductSolveConcreteProblemQuestion','CanYouProveThatProblemExistsQuestion','LinksToProblemDescriptionQuestion','WaysToSolveProblemWithoutBlockchainQuestion','ThreeWaysToUseThisProductQuestion','DoYouHaveBusinessModelQuestion','BusinessModelDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 56,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('ClientOfferValueQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 57,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('TargetAudienceInterestProveQuestion','TargetAudienceInterestDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 58,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveRealUsersQuestion','HowMuchRealUsersDoYouHaveQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 59,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('YourCompetitorsThatUseBlockchainQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 59,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('YourCompetitorsThatUseBlockchainQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 60,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('YourCompetitorsThatUseBlockchainQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 61,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('YourStrengthsQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 62,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('YourWeaknessQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 63,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('YourProjectOpportunitiesQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 64,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('YourProjectRisksQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 65,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('YourProjectBenefitFromBlockchainQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 66,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('DoYouHaveUniqueTechnologyQuestion','UniqueTechnologyDescriptionQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 67,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('CollectingFundsPlanQuestion','MinimumIcoRaiseAmountQuestion','MaximumIcoRaiseAmountQuestion','IcoRaisedFundsSpendPurposeQuestion')

                                INSERT INTO ScoringCriteriaMappings (ScoringCriterionId,ScoringApplicationQuestionId)
                                SELECT 68,saq.Id
                                FROM ScoringApplicationQuestions saq
                                WHERE [Key] in ('HoldersBenefitFromProjectTokenQuestion')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoringCriteriaMappings");
        }
    }
}
