import {Injectable} from '@angular/core';
import {ExpertiseArea} from '../api/scoring/expertise-area.enum';
import {HttpClient} from '@angular/common/http';
import {BaseApiClient} from '../api/base-api-client';
import {CollectionResponse} from '../api/collection-response';
import {QuestionResponse} from '../api/estimates/question-response';

@Injectable()
export class QuestionService extends BaseApiClient {
  private questions: { [expertType: number]: Array<QuestionResponse>; } = {};

  constructor(private http: HttpClient) {
    super();
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

  public async initializeQuestionsCollection(): Promise<void> {
    const allQuestions = await this.http.get<CollectionResponse<QuestionResponse>>(this.baseApiUrl + '/estimates/questions').toPromise();
    for (const item in ExpertiseArea) {
      if (Number(item)) {
        this.questions[item] = allQuestions.items.filter(i => i.expertiseArea === parseInt(item, 0));
      }
    }
  }
}
