import {Injectable} from '@angular/core';
import {Question} from './question';
import {ExpertiseArea} from '../api/scoring/expertise-area.enum';
import {HttpClient} from '@angular/common/http';
import {BaseApiClient} from '../api/base-api-client';
import {GetQuestionsResponse} from '../services/get-questions-response';
import {isObject} from 'rxjs/util/isObject';
import {QuestionResponse} from '../api/estimates/question-response';
import {Estimate} from './estimate';


@Injectable()
export class QuestionService extends BaseApiClient {
  private questions: { [expertType: number]: Array<QuestionResponse>; } = {};

  constructor(private http: HttpClient) {
    super();
  }

  public async getByCategory(category: ExpertiseArea): Promise<Array<QuestionResponse>> {
    await this.checkAndFill();
    return this.questions[category];
  }

  private async checkAndFill() {
    if (isObject(this.questions)) {
      await this.initializeQuestionsCollection();
    }
  }


  public async getMaxScoreForCategory(category: ExpertiseArea): Promise<number> {
    await this.checkAndFill();
    const categoryQuestions = this.questions[category];
    return categoryQuestions.map(q => q.maxScore).reduce((previous, current) => previous + current);
  }

  public async getMinScoreForCategory(category: ExpertiseArea): Promise<number> {
    await this.checkAndFill();
    const categoryQuestions = this.questions[category];
    return categoryQuestions.map(q => q.minScore).reduce((previous, current) => previous + current);
  }

  private async initializeQuestionsCollection() {
    const allQuestions = await this.http.get<GetQuestionsResponse>(this.baseApiUrl + '/estimates/questions').toPromise();
    for (const item in ExpertiseArea) {
      if (Number(item)) {
        this.questions[item] = allQuestions.items;
      }
    }
  }
}
