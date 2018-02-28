import {Injectable} from '@angular/core';
import {AreaType} from '../../api/scoring/area-type.enum';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {QuestionResponse} from '../../api/estimates/question-response';
import {TranslateService} from '@ngx-translate/core';

@Injectable()
export class QuestionService {
  private questions: { [areaType: number]: Array<QuestionResponse>; } = {};

  constructor(private estimatesClient: EstimatesApiClient,
              private translateService: TranslateService) {
  }

  public getByAreaType(areaType: AreaType): Array<QuestionResponse> {
    return this.questions[areaType];
  }

  public getMaxScoreForArea(areaType: AreaType): number {
    const categoryQuestions = this.questions[areaType];
    return categoryQuestions.map(q => q.maxScore).reduce((previous, current) => previous + current);
  }

  public getMinScoreForArea(areaType: AreaType): number {
    const categoryQuestions = this.questions[areaType];
    return categoryQuestions.map(q => q.minScore).reduce((previous, current) => previous + current);
  }

  public async initializeAsync(): Promise<void> {
    const allQuestions = await this.estimatesClient.getQuestionsAsync();
    for (const question of allQuestions.items) {
      question.name = await this.translateService.get('QuestionsNames.' + question.id).toPromise();
      question.description = await this.translateService.get('QuestionsDescriptions.' + question.id).toPromise();
    }
    for (const item in AreaType) {
      if (Number(item)) {
        this.questions[item] = allQuestions.items.filter(i => i.areaType === parseInt(item, 0));
      }
    }
  }
}
