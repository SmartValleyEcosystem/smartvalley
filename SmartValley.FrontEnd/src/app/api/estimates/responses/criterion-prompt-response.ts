import {CriterionPrompt} from '../criterion-prompt';

export interface CriterionPromptResponse {
  scoringCriterionId: number;
  prompts: CriterionPrompt[];
}
