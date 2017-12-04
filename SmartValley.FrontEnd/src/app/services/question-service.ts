import {Injectable} from '@angular/core';
import {Question} from './question';
import {ExpertiseArea} from '../api/scoring/expertise-area.enum';
import {HttpClient} from '@angular/common/http';
import {BaseApiClient} from '../api/base-api-client';


@Injectable()
export class QuestionService extends BaseApiClient {
  private questions: { [expertType: number]: Array<Question>; } = {};

  constructor(private http: HttpClient) {
    super();
    this.initializeQuestionsCollection();
  }

  public getByCategory(category: ExpertiseArea): Array<Question> {
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

  private async initializeQuestionsCollection() {
    const allQuestions = await this.http.get<Array<Question>>(this.baseApiUrl + '/questions').toPromise();
    for (const item in ExpertiseArea) {
      if (isNaN(Number(item))) {
        this.questions[item] = allQuestions.filter(v => v.expertType === parseInt(item, 0));
      }
    }
  }
}
