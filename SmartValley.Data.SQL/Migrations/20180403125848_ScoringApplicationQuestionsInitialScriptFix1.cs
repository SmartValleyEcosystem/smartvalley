using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class ScoringApplicationQuestionsInitialScriptFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from [dbo].[ScoringApplicationQuestions]");

            migrationBuilder.Sql(@"
declare @Id bigint
INSERT[dbo].[ScoringApplicationQuestions]([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveUniqueTechnologyQuestion', N'', NULL, NULL, N'ProjectGroup', 1, 4, 1)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'UniqueTechnologyDescriptionQuestion', N'', @Id, N'1', N'ProjectGroup', 1, 1, 2)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'FinancialFundsDescriptionQuestion', N'', NULL, NULL, N'ProjectGroup', 1, 1, 3)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouhaveICORoadmapQuestion', N'', NULL, NULL, N'ProjectGroup', 1, 4, 4)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'ICORoadmapDescriptionLinkQuestion', N'', @Id, N'1', N'ProjectGroup', 1, 5, 5)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouNeedAdditionalEmployeesQuestion', N'', NULL, NULL, N'TeamGroup', 2, 4, 1)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'AdditionalEmployeesWorkplaceDescriptionQuestion', N'', @Id, N'1', N'TeamGroup', 2, 1, 2)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouUseOutsourcingQuestion', N'', NULL, NULL, N'TeamGroup', 2, 4, 3)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DescribeOutsourcingPurposeQuestion', N'', @Id, N'1', N'TeamGroup', 2, 1, 4)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoesYourTeamHasExperienceInProjectAreaQuestion', N'', NULL, NULL, N'TeamGroup', 2, 4, 5)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TeamProjectAreaExperienceDescriptionQuestion', N'', @Id, N'1', N'TeamGroup', 2, 1, 6)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoesYourTeamMembersHaveOtherIcoProjectExperienceQuestion', N'', NULL, NULL, N'TeamGroup', 2, 4, 7)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TeamMembersOtherIcoProjectExperienceDesciprionQuestion', N'', @Id, N'1', N'TeamGroup', 2, 1, 8)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoesYourTeamMembersHaveFundingAttractingExperienceQuestion', N'', NULL, NULL, N'TeamGroup', 2, 4, 9)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TeamMembersFundingAttractingExperienceDescriptionQuestion', N'', @Id, 1, N'TeamGroup', 2, 1, 10)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoesYourTeamMembersHaveFinancialOffencesQuestion', N'', NULL, NULL, N'TeamGroup', 2, 4, 11)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TeamMembersFinancialOffencesDescriptionQuestion', N'', @Id, N'1', N'TeamGroup', 2, 1, 12)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'IsItPossibleToAttractAdditionalFundsQuestion', N'', NULL, NULL, N'BusinessFinanceGroup', 3, 1, 1)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'HowMuchMoneyInvestedQuestion', N'', NULL, NULL, N'BusinessFinanceGroup', 3, 0, 2)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveFinancialModelQuestion', N'', NULL, NULL, N'BusinessFinanceGroup', 3, 4, 3)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'FinancialModelLinkQuestion', N'', @Id, N'1', N'BusinessFinanceGroup', 3, 5, 4)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'LinksToFinancialModelResearchQuestion', N'', NULL, NULL, N'BusinessFinanceGroup', 3, 1, 5)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'CurrentlyAvailableResources', N'{""Values"": [""OwnedCapitalAnswer"", ""AttractedCapitalAnswer"", ""OwnedAndAttractedCapitalAnswer"", ""HasNoCapitalAnswer""]}', NULL, NULL, N'BusinessFinanceGroup', 3, 2, 6)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'UsedBlockchainTypeQuestion', N'', NULL, NULL, N'TechnicalDetailsGroup', 4, 1, 1)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveAlgorithmsSourceCodeQuestion', N'', NULL, NULL, N'TechnicalDetailsGroup', 4, 4, 2)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'AlgorithmsSourceCodeLinkQuestion', N'', @Id, N'1', N'TechnicalDetailsGroup', 4, 5, 3)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveFuturePlatformTechnicalDescriptionQuestion', N'', NULL, NULL, N'TechnicalDetailsGroup', 4, 4, 4)

 set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'FuturePlatformTechnicalDescriptionLinkQuestion', N'', @Id, N'1', N'TechnicalDetailsGroup', 4, 5, 5)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'PlanningCountUsersToAttractQuestion', N'', NULL, NULL, N'TechnicalDetailsGroup', 4, 0, 6)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'PlanningPayloadQuestion', N'', NULL, NULL, N'TechnicalDetailsGroup', 4, 0, 7)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'PlanForPayloadIncreasingQuestion', N'', NULL, NULL, N'TechnicalDetailsGroup', 4, 1, 8)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveMVPQuestion', N'', NULL, NULL, N'PrototypeMVPGroup', 5, 4, 1)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'MVPFeatureDescriptionQuestion', N'', @Id, N'1', N'PrototypeMVPGroup', 5, 1, 2)

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'LinkToMVPQuestion', N'', @Id, N'1', N'PrototypeMVPGroup', 5, 5, 3)

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'MVPSpendedMoneyQuestion', N'', @Id, N'1', N'PrototypeMVPGroup', 5, 0, 4)

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'MVPTimeSpendedQuestion', N'', @Id, N'1', N'PrototypeMVPGroup', 5, 0, 5)

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'PlannedMVPFeatureDescriptionQuestion', N'', @Id, N'0', N'PrototypeMVPGroup', 5, 1, 6)

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'MVPPlanningSpendedMoneyQuestion', N'', @Id, N'0', N'PrototypeMVPGroup', 5, 0, 7)

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'MVPPlanningTimeSpendedQuestion', N'', @Id, N'0', N'PrototypeMVPGroup', 5, 0, 8)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveMVPSourceCodesQuestion', N'', NULL, NULL, N'PrototypeMVPGroup', 5, 4, 9)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'MVPSourceCodesLinkQuestion', N'', @Id, N'1', N'PrototypeMVPGroup', 5, 0, 10)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'CollectingFundsPlanQuestion', N'', NULL, NULL, N'ICOGroup', 6, 1, 1)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'MinimumIcoRaiseAmountQuestion', N'', NULL, NULL, N'ICOGroup', 6, 0, 2)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'MaximumIcoRaiseAmountQuestion', N'', NULL, NULL, N'ICOGroup', 6, 0, 3)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'IcoRaisedFundsSpendPurposeQuestion', N'', NULL, NULL, N'ICOGroup', 6, 1, 4)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TokenDistributionPlanQuestion', N'', NULL, NULL, N'ICOGroup', 6, 1, 5)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'CrowdsaleDescriptionQuestion', N'', NULL, NULL, N'ICOGroup', 6, 1, 6)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TokenFunctionsDescriptionQuestion', N'', NULL, NULL, N'TokenGroup', 7, 1, 1)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TokenTypeQuestion', N'{""Values"": [""UtilityTokenTypeAnswer"", ""SecurityTokenTypeAnswer""]}', NULL, NULL, N'TokenGroup', 7, 2, 2)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'LicensingForSecurityTokenQuestion', N'', NULL, NULL, N'TokenGroup', 7, 1, 3)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'BuyerTokenValueQuestion', N'', NULL, NULL, N'TokenGroup', 7, 1, 4)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TokenValueGrowthFactorsQuestion', N'', NULL, NULL, N'TokenGroup', 7, 1, 5)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TokenNecessityForProjectQuestion', N'', NULL, NULL, N'TokenGroup', 7, 1, 6)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TokenChangingToETHQuestion', N'', NULL, NULL, N'TokenGroup', 7, 1, 7)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveTokenEmissionRestrictionQuestion', N'', NULL, NULL, N'TokenGroup', 7, 4, 8)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TokenEmissionRestrictionDescriptionQuestion', N'', @Id, N'1', N'TokenGroup', 7, 0, 9)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TokenPriceQuestion', N'', NULL, NULL, N'TokenGroup', 7, 0, 10)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TokenQuantityLimitationQuestion', N'', NULL, NULL, N'TokenGroup', 7, 0, 11)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'WhenUserCanUseTokensQuestion', N'', NULL, NULL, N'TokenGroup', 7, 1, 12)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoesYourProductSolveConcreteProblemQuestion', N'', NULL, NULL, N'ProductGroup', 8, 1, 1)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'CanYouProveThatProblemExistsQuestion', N'', NULL, NULL, N'ProductGroup', 8, 4, 2)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'LinksToProblemDescriptionQuestion', N'', @Id, N'1', N'ProductGroup', 8, 1, 3)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'WaysToSolveProblemWithoutBlockchainQuestion', N'', NULL, NULL, N'ProductGroup', 8, 1, 4)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'ThreeWaysToUseThisProductQuestion', N'', NULL, NULL, N'ProductGroup', 8, 1, 5)
GO

declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveBusinessModelQuestion', N'', NULL, NULL, N'ProductGroup', 8, 4, 6)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'BusinessModelDescriptionQuestion', N'', @Id, N'1', N'ProductGroup', 8, 1, 7)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHavePartnershipsQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 4, 1)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'PartnershipBenefitDescriptionQuestion', N'', @Id, N'1', N'MarketingGroup', 9, 1, 2)
GO
declare @Id bigint
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveMarketingPlanQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 4, 3)

 set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'LinkToMarketingPlanQuestion', N'', @Id, N'1', N'MarketingGroup', 9, 5, 4)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TargetMarketSizeQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 0, 5)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TypicalProductUserDescriptionQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 6)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TargetAudienceQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 7)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'MarketGeographyQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 8)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'ClientOfferValueQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 9)
GO
declare @Id bigint
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TargetAudienceInterestProveQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 4, 10)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TargetAudienceInterestDescriptionQuestion', N'', @Id, N'1', N'MarketingGroup', 9, 1, 11)
GO
declare @Id bigint
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveRealUsersQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 4, 12)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'HowMuchRealUsersDoYouHaveQuestion', N'{""Values"": [""0RealUsersAnswer"", ""0_100RealUsersAnswer"", ""100_500RealUsersAnswer"", ""500_1000RealUsersAnswer"", ""1000_10000RealUsersAnswer"", ""10000+RealUsersAnswer""]}', @Id, N'1', N'MarketingGroup', 9, 2, 13)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'YourCompetitorsThatUseBlockchainQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 14)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'YourCompetitorsThatDoNotUseBlockchainQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 15)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'YourStrengthsQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 16)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'YourWeaknessQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 17)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'YourProjectOpportunitiesQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 18)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'YourProjectRisksQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 19)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'YourProjectBenefitFromBlockchainQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 20)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'HoldersBenefitFromProjectTokenQuestion', N'', NULL, NULL, N'MarketingGroup', 9, 1, 21)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveTokenSaleAgreementQuestion', N'', NULL, NULL, N'LegalGroup', 10, 4, 1)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TokenSaleAgreementLinkQuestion', N'', @Id, N'1', N'LegalGroup', 10, 5, 2)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveLegalOpinionQuestion', N'', NULL, NULL, N'LegalGroup', 10, 4, 3)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'LegalOpinionLinkQuestion', N'', @Id, N'1', N'LegalGroup', 10, 5, 4)
GO
declare @Id bigint

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveAgreementOfRiskAssessmentQuestion', N'', NULL, NULL, N'LegalGroup', 10, 4, 5)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'AgreementOfRiskAssessmentLinkQuestion', N'', @Id, N'1', N'LegalGroup', 10, 5, 6)
GO
declare @Id bigint
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouHaveTermsAndConditionsQuestion', N'', NULL, NULL, N'LegalGroup', 10, 4, 7)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'TermsAndConditionsLinkQuestion', N'', @Id, N'1', N'LegalGroup', 10, 5, 8)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'CountriesNotForFundingQuestion', N'', NULL, NULL, N'LegalGroup', 10, 1, 9)
GO
declare @Id bigint
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'HaveYouEverMadeProfitPromisesQuestion', N'', NULL, NULL, N'LegalGroup', 10, 4, 10)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'ProfitPromisesDescriptionQuestion', N'', @Id, N'1', N'LegalGroup', 10, 1, 11)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'MarketingActivitiesCountriesQuestion', N'', NULL, NULL, N'LegalGroup', 10, 1, 12)
GO
declare @Id bigint
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'HaveYouEverMadeStockExchangePromisesQuestion', N'', NULL, NULL, N'LegalGroup', 10, 4, 13)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'StockExchangePromisesDescriptionQuestion', N'', @Id, N'1', N'LegalGroup', 10, 1, 14)
GO
declare @Id bigint
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'WillYouArrangeKYCQuestion', N'', NULL, NULL, N'LegalGroup', 10, 4, 15)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'KYCDescriptionQuestion', N'', @Id, N'1', N'LegalGroup', 10, 1, 16)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'CountryOfIncorporationQuestion', N'', NULL, NULL, N'LegalGroup', 10, 0, 17)
GO
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'LinkToCompanyRegistrationDocumentQuestion', N'', NULL, NULL, N'LegalGroup', 10, 5, 18)
GO
declare @Id bigint
INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'DoYouNeedAdditionalLicencesQuestion', N'', NULL, NULL, N'LegalGroup', 10, 4, 19)

set @Id = @@IDENTITY

INSERT[dbo].[ScoringApplicationQuestions]
        ([Key], [ExtendedInfo], [ParentId], [ParentTriggerValue], [GroupKey], [GroupOrder], [Type], [Order])
VALUES(N'AdditionalLicencesDescriptionQuestion', N'', @Id, N'1', N'LegalGroup', 10, 1, 20)
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
