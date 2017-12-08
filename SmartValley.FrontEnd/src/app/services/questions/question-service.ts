import {Injectable} from '@angular/core';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {QuestionResponse} from '../../api/estimates/question-response';
import {TranslateService} from '@ngx-translate/core';

@Injectable()
export class QuestionService {
  private questions: { [expertiseArea: number]: Array<QuestionResponse>; } = {};

  constructor(private estimatesClient: EstimatesApiClient,
              private translate: TranslateService) {
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

  public async initializeAsync(): Promise<void> {
    const allQuestions = await this.estimatesClient.getQuestionsAsync();
    for (const item in ExpertiseArea) {
      if (Number(item)) {
        this.questions[item] = allQuestions.items.filter(i => i.expertiseArea === parseInt(item, 0));
      }
    }
  }
}
