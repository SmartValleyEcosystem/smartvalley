import {AdviserResponse} from '../scoring-application/adviser-response';
import {TeamMember} from '../scoring-application/team-member';
import {SocialNetworksResponse} from '../scoring-application/social-networks-response';
import {QuestionControlType} from '../scoring-application/question-control-type.enum';

export interface CriterionPrompt {
  title: string;
  answer: string;
  projectTeamMembers: TeamMember[];
  adviserResponse: AdviserResponse;
  socialNetworks: SocialNetworksResponse;
  scoringCriterionPromptType: any;
  questionControlType: QuestionControlType;
}
