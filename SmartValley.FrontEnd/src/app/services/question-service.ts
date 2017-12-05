import {Injectable} from '@angular/core';
import {ExpertiseArea} from '../api/scoring/expertise-area.enum';
import {EstimatesApiClient} from '../api/estimates/estimates-api-client';
import {QuestionResponse} from '../api/estimates/question-response';

@Injectable()
export class QuestionService {
  private questions: { [expertType: number]: Array<QuestionResponse>; } = {};

  constructor(private estimatesClient: EstimatesApiClient) {
  }

  public getByExpertiseArea(expertiseArea: ExpertiseArea): Array<QuestionResponse> {
    return this.questions[expertiseArea];
  }

  public getMaxScoreForExpertiseArea(expertiseArea: ExpertiseArea): number {
    const categoryQuestions = this.questions[expertiseArea];
    return categoryQuestions.map(q => q.maxScore).reduce((previous, current) => previous + current);
  }

  public getMinScoreForExpertiseArea(expertiseArea: ExpertiseArea): number {
    const categoryQuestions = this.questions[expertiseArea];
    return categoryQuestions.map(q => q.minScore).reduce((previous, current) => previous + current);
  }

  public async initializeQestionsCollectionAsync(): Promise<void> {
    const allQuestions = await this.estimatesClient.getQuestions();
    for (const item in ExpertiseArea) {
      if (Number(item)) {
        this.questions[item] = allQuestions.items.filter(i => i.expertiseArea === parseInt(item, 0));
      }
    }
  }
}
