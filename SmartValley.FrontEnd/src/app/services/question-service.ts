import {Injectable} from '@angular/core';
import {ExpertiseArea} from '../api/scoring/expertise-area.enum';
import {HttpClient} from '@angular/common/http';
import {BaseApiClient} from '../api/base-api-client';
import {GetQuestionsResponse} from '../services/get-questions-response';
import {QuestionResponse} from '../api/estimates/question-response';

@Injectable()
export class QuestionService extends BaseApiClient {
  private questions: { [expertType: number]: Array<QuestionResponse>; } = {};

  constructor(private http: HttpClient) {
    super();
  }

  public getByCategory(category: ExpertiseArea): Array<QuestionResponse> {
    return this.questions[category];
  }

  public getMaxScoreForCategory(category: ExpertiseArea): number {
    const categoryQuestions = this.questions[category];
    return categoryQuestions.map(q => q.maxScore).reduce((previous, current) => previous + current);
  }

  public getMinScoreForCategory(category: ExpertiseArea): number {
    const categoryQuestions = this.questions[category];
    return categoryQuestions.map(q => q.minScore).reduce((previous, current) => previous + current);
  }

  public async initializeQuestionsCollection() {
    const allQuestions = await this.http.get<GetQuestionsResponse>(this.baseApiUrl + '/estimates/questions').toPromise();
    for (const item in ExpertiseArea) {
      if (Number(item)) {
        this.questions[item] = allQuestions.items;
      }
    }
  }
}
