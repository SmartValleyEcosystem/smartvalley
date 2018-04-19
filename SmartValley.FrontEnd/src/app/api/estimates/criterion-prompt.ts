import {AdviserResponse} from '../scoring-application/adviser-response';
import {TeamMember} from '../scoring-application/team-member';
import {SocialNetworksResponse} from '../scoring-application/social-networks-response';

export interface CriterionPrompt {
  title: string;
  answer: string;
  ProjectTeamMembers: TeamMember[];
  AdviserResponse: AdviserResponse;
  SocialNetworks: SocialNetworksResponse;
  ScoringCriterionPromptType: any;
}
